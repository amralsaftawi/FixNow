FixNow — Database Architecture Review
Initial EF Core Migration — Pre-Launch Design Review
Prepared for: Engineering Leadership  |  Reviewer: Principal Database Architect  |  Date: August 1, 2026
1. Executive Summary
FixNow's initial schema models a service marketplace (customers, technicians, service requests, assignments, payments, reviews) with JWT + refresh-token authentication and OTP verification. Overall, the migration reflects a solid, deliberate first pass: audit columns are applied consistently, timestamps correctly use timestamptz, money and coordinates use appropriate numeric precision, and the customer/technician profile-extension pattern is a clean modeling choice.
However, the schema is not yet production-ready for a system targeting millions of users. The most serious gaps are structural integrity guarantees the database is not currently enforcing: nothing prevents a user from having duplicate customer or technician profiles, nothing prevents duplicate accounts on the same phone number or email, and there are no CHECK constraints anywhere in the schema. These are exactly the kinds of gaps that are cheap to fix before launch and very expensive to fix once real data and real users exist.
A secondary set of findings concerns PostgreSQL-specific performance characteristics at scale — random UUIDv4 primary keys on high-write tables, missing composite indexes for the platform's actual query patterns, and no partitioning plan for high-churn, time-ordered tables like refresh tokens and OTP records.
Overall Score: 5.5 / 10
The schema is a workable foundation, not a launch-ready one. None of the findings below require a redesign — they are targeted, additive changes (constraints, indexes, and a handful of column additions) that should be resolved before the first production deployment.
2. Findings
Twenty findings are detailed below, ordered by severity. A summary table is provided first for quick reference, followed by full detail on each finding.
ID	Severity	Category	Finding
F-01	Critical	Integrity	No unique constraint on CustomerProfiles.UserId / TechnicianProfiles.UserId
F-02	Critical	Integrity/Security	No unique index on Users.Email / Users.PhoneNumber
F-03	High	Integrity	No uniqueness guard on Reviews.AssignmentId (duplicate reviews)
F-04	High	Design	No CHECK constraints anywhere in the schema
F-05	High	PostgreSQL / Performance	Random UUIDv4 primary keys on high-write tables
F-06	High	Performance	Missing status / composite indexes for core query patterns
F-07	Medium	Security	No unique index on RefreshTokenHash; unguarded self-referencing user FKs
F-08	Medium	Design	Enum-like fields stored as unconstrained plain integers
F-09	Medium	Design / Performance	Address geography fields lack referential integrity and geospatial indexing
F-10	Medium	Scalability	No Currency column on Payments ahead of multi-country rollout
F-11	Medium	Maintainability	CreatedBy / LastModifiedBy stored as free text, not FK
F-12	Medium	Performance/Integrity	No optimistic concurrency token on mutable entities
F-13	Low	Design	Inconsistent soft-delete convention (DeletedAt vs IsActive)
F-14	Low	PostgreSQL	Oversized varchar lengths on hash columns
F-15	Medium	Integrity	UserRoles unique index blocks role re-assignment after revocation
F-16	Medium	EF Core	Uniform Restrict delete behavior on dependent-only child tables
F-17	Low	Security	Sensitive PII stored without visible classification/retention controls
F-18	Low	Scalability	Single default schema ahead of planned microservice decomposition
F-19	Medium	Performance	No partitioning strategy for high-volume, time-ordered tables
F-20	Low	PostgreSQL	Payments table missing gateway reference / idempotency key


2.1 Detailed Findings
F-01 — Missing 1:1 uniqueness on CustomerProfiles.UserId and TechnicianProfiles.UserId
Severity	Critical
Category	Integrity

Description: CustomerProfiles and TechnicianProfiles are modeled as a one-to-one extension of Users, but the migration only creates a non-unique index (IX_CustomerProfiles_UserId, IX_TechnicianProfiles_UserId) on the foreign key. There is no unique constraint enforcing that a given user can have at most one profile of each type.
Why it is a problem: The requirements document explicitly lists "multiple customer profiles" as a business rule the schema must prevent. As written, nothing at the database layer stops two CustomerProfiles rows (or two TechnicianProfiles rows) from pointing at the same UserId. Application-level checks can be bypassed by race conditions, bulk imports, background jobs, or future services that write directly to the table.
Real-world impact: Duplicate profiles fragment a user's order/review/rating history across two rows, break "one active technician profile" assumptions used by matching and payout logic, and are very difficult to safely merge once millions of rows exist.
Recommended solution: Convert both indexes to unique indexes on UserId. This is the single highest-priority fix in the entire schema.
Example:
migrationBuilder.CreateIndex(name: "IX_CustomerProfiles_UserId", table: "CustomerProfiles", column: "UserId", unique: true);
F-02 — No unique index on Users.Email or Users.PhoneNumber
Severity	Critical
Category	Integrity / Security

Description: Users.Email (nullable) and Users.PhoneNumber (required) have no unique constraints anywhere in the migration — only the primary key on Id is unique.
Why it is a problem: Phone number is clearly the primary identity/login credential in an OTP-driven system, and duplicate accounts under the same phone or email defeat both authentication and OTP verification (a fresh OTP for one duplicate does not verify the other), and let a banned or fraudulent user simply re-register.
Real-world impact: At marketplace scale this becomes an abuse and support nightmare: duplicate accounts split rating/trust history, break "one free trial" or referral logic, and multiply OTP/SMS costs for the same physical person. Retrofitting a unique constraint after millions of rows already contain duplicates is a painful, multi-day data-cleanup migration.
Recommended solution: Add a unique index on PhoneNumber (mandatory field). For Email, add a partial unique index that ignores NULLs, since it is optional: CREATE UNIQUE INDEX ... WHERE "Email" IS NOT NULL. Consider also excluding soft-deleted rows (WHERE "DeletedAt" IS NULL) so a phone number can be reused after an account is deactivated.
Example:
migrationBuilder.CreateIndex(name: "IX_Users_PhoneNumber", table: "Users", column: "PhoneNumber", unique: true, filter: "\"DeletedAt\" IS NULL");
migrationBuilder.CreateIndex(name: "IX_Users_Email", table: "Users", column: "Email", unique: true, filter: "\"Email\" IS NOT NULL AND \"DeletedAt\" IS NULL");
F-03 — No uniqueness guard against duplicate Reviews per Assignment
Severity	High
Category	Integrity

Description: Reviews carries AssignmentId, ServiceRequestId, CustomerProfileId and TechnicianProfileId, all with plain (non-unique) indexes. Nothing prevents inserting more than one Review row for the same Assignment.
Why it is a problem: The requirements explicitly call out "multiple reviews" as a domain rule to enforce. Without a database-level guard, a retried API call, a buggy mobile client, or a race condition between two devices can insert duplicate reviews.
Real-world impact: Duplicate reviews double-count in a technician's average rating, are visible to customers as spam, and require manual data cleanup plus a rating-recalculation job to fix after the fact.
Recommended solution: Add a unique index on AssignmentId (assuming the business rule is one review per completed assignment).
Example:
migrationBuilder.CreateIndex(name: "IX_Reviews_AssignmentId", table: "Reviews", column: "AssignmentId", unique: true);
F-04 — No CHECK constraints anywhere in the schema
Severity	High
Category	Design

Description: Across all 17 tables, there is not a single CHECK constraint. Several columns have an obvious valid range that is currently enforced only in application code: Reviews.Rating, OtpRecords.AttemptsCount vs MaxAttempts, Payments.Amount, and Addresses.Latitude/Longitude.
Why it is a problem: Application-level validation does not protect the database from bad data introduced by migrations, admin tools, bulk-import scripts, hot-fixes run directly against the database, or a future microservice that writes to the same tables with slightly different validation logic. Constraints are the last line of defense for data integrity and, unlike application code, cannot be forgotten in a new code path.
Real-world impact: Silent bad data (e.g. Rating = 0 or Rating = 47, a negative Payments.Amount, AttemptsCount exceeding MaxAttempts) corrupts aggregates such as average technician rating and financial reconciliation reports, and is often only discovered much later during an audit.
Recommended solution: Add CHECK constraints for every bounded numeric/business-rule column.
Example:
migrationBuilder.Sql("ALTER TABLE \"Reviews\" ADD CONSTRAINT \"CK_Reviews_Rating\" CHECK (\"Rating\" BETWEEN 1 AND 5);");
migrationBuilder.Sql("ALTER TABLE \"Payments\" ADD CONSTRAINT \"CK_Payments_Amount\" CHECK (\"Amount\" > 0);");
migrationBuilder.Sql("ALTER TABLE \"OtpRecords\" ADD CONSTRAINT \"CK_OtpRecords_Attempts\" CHECK (\"AttemptsCount\" <= \"MaxAttempts\");");
F-05 — Random UUIDv4 primary keys on high-write tables
Severity	High
Category	PostgreSQL / Performance

Description: Every table uses a uuid primary key, and nothing in the migration indicates the application generates them sequentially (e.g. UUIDv7 / ordered GUIDs). Tables such as RefreshTokens, OtpRecords, UserSessions, ServiceRequestTimelines and Assignments will receive very high insert volumes as the platform grows.
Why it is a problem: Random UUIDs scatter inserts across the entire B-tree range of the primary key index, defeating PostgreSQL's append-mostly write pattern. This causes index bloat, more random I/O, worse buffer-cache locality, and larger WAL volume compared to a sequential key — a well-documented PostgreSQL scaling pitfall at the "millions of rows" scale this project targets.
Real-world impact: At 10M+ users generating sessions, OTPs, refresh tokens and assignments continuously, this shows up as steadily degrading insert latency and bloated indexes that need frequent REINDEX/VACUUM maintenance, directly impacting login and OTP verification latency — the most latency-sensitive paths in the app.
Recommended solution: Switch to time-ordered identifiers (UUIDv7, or NHibernate/EF "sequential GUID" generators) for high-volume tables, or use a bigint identity column as the physical primary key with a separate random uuid "public id" column for external exposure.
F-06 — Missing status / composite indexes for core query patterns
Severity	High
Category	Performance

Description: ServiceRequests, Assignments and Payments each expose only single-column FK indexes (CustomerProfileId, AddressId, ServiceCategoryId, ServiceRequestId, TechnicianProfileId, AssignmentId). None of the frequently-filtered Status columns are indexed, and no composite indexes exist to support the platform's core queries.
Why it is a problem: A service marketplace's hottest read paths are exactly the ones this schema cannot serve efficiently: "find all Pending ServiceRequests in a category," "find all Assignments a technician currently has Active," "find all Payments that are still Pending/Failed." Without a supporting index, PostgreSQL falls back to a sequential scan (or a large partial scan of the FK index) once these tables reach millions of rows.
Real-world impact: Dispatch/matching queries and technician/customer dashboards degrade from milliseconds to seconds as data grows, and the degradation is gradual and easy to miss in staging with small seed data — it typically surfaces in production under real load.
Recommended solution: Add composite indexes aligned to real query shapes, e.g. (Status, ServiceCategoryId) on ServiceRequests, (TechnicianProfileId, Status) on Assignments, and (Status) or (Status, CreatedAtUtc) on Payments.
Example:
migrationBuilder.CreateIndex(name: "IX_ServiceRequests_Status_ServiceCategoryId", table: "ServiceRequests", columns: new[] { "Status", "ServiceCategoryId" });
migrationBuilder.CreateIndex(name: "IX_Assignments_TechnicianProfileId_Status", table: "Assignments", columns: new[] { "TechnicianProfileId", "Status" });
F-07 — RefreshTokenHash has no unique index; audit user references are unguarded
Severity	Medium
Category	Security

Description: RefreshTokens.RefreshTokenHash has no unique constraint, so a hash collision or a bug that regenerates the same hash would silently create two valid sessions for the same token. Separately, UserRoles.AssignedByUserId / RevokedByUserId and RefreshTokens/UserSessions-style "actor" columns are plain nullable Guid columns with no foreign key back to Users, so they can reference a non-existent user id.
Why it is a problem: A unique constraint on the token hash is what actually turns "lookup by hash" into an O(1), tamper-evident operation and prevents duplicate-token edge cases used in some replay-detection strategies. Un-enforced actor references silently degrade the audit trail this table was clearly designed to provide (who assigned/revoked a role) — exactly the kind of gap that surfaces during a security incident review when it's too late to fix.
Real-world impact: Weakens the ability to detect refresh-token replay/reuse (a standard OAuth2 refresh-token-rotation security control), and produces an audit trail that cannot be trusted for compliance or incident response.
Recommended solution: Add a unique index on RefreshTokenHash, and add foreign keys (ReferentialAction.Restrict or SetNull) from AssignedByUserId / RevokedByUserId back to Users.Id.
F-08 — Enum-like fields stored as unconstrained plain integers
Severity	Medium
Category	Design

Description: Fields such as PreferredLanguage, RegisteredVia, AccountStatus, VerificationStatus, Availability, Purpose, PaymentMethod, Status (on ServiceRequests/Assignments/Payments), Priority, CancellationReason and RejectReason are all mapped as bare integer columns with no CHECK constraint, native PostgreSQL enum, or reference/lookup table.
Why it is a problem: The database cannot reject an out-of-range value (e.g. Status = 999), which means invalid state can only ever be caught by application code — and, per the project's own architecture (Clean Architecture + CQRS across a future set of microservices), that guarantee will not hold once more than one service or reporting tool writes to these tables.
Real-world impact: Invalid enum values silently break state-machine assumptions in the domain layer (e.g. an Assignment with an impossible Status), and are hard to detect until a report or a background job crashes trying to interpret the value.
Recommended solution: For a genuinely fixed, rarely-changing set of values, use a native PostgreSQL enum type or a CHECK IN (...) constraint. For values that are more country/config-driven (e.g. CancellationReason, PaymentMethod), promote them to small reference/lookup tables with FKs — this also gives you a place to attach translations for the planned multi-language support.
F-09 — Address geography fields lack referential integrity and geospatial indexing
Severity	Medium
Category	Design / Performance

Description: Addresses.CountryId, CityId and AreaId are plain integer columns with no corresponding Countries/Cities/Areas tables in this migration and no foreign keys. Latitude/Longitude are stored as numeric(9,6) with no spatial index.
Why it is a problem: Without reference tables, there is nothing stopping an invalid CountryId/CityId/AreaId combination, and no natural place to hang multi-language city/area names for the planned internationalization. Storing coordinates as plain numeric means "find technicians within N km" — the single most fundamental query in a service-dispatch marketplace — must be computed with an unindexed Haversine formula in application code or a slow bounding-box scan.
Real-world impact: As the technician/customer base grows into the millions, proximity-based matching queries will scan far more rows than necessary and will not scale past a fairly small metro population without a rewrite.
Recommended solution: Introduce Countries/Cities/Areas reference tables with proper FKs, and adopt PostGIS's geography(Point,4326) type with a GiST index for Latitude/Longitude (or an additional generated geography column derived from the existing numeric fields) to support efficient radius/"nearest technician" queries.
F-10 — No Currency column on Payments ahead of multi-country rollout
Severity	Medium
Category	Scalability

Description: Payments.Amount is numeric(12,2) with no accompanying currency code. The project's stated roadmap explicitly includes multiple countries.
Why it is a problem: A single-currency assumption baked into the schema today becomes a breaking, high-risk migration later — every historical Payments row will need a currency backfilled, and every downstream report or reconciliation query will need to be rewritten with an assumption about which rows are in which currency.
Real-world impact: Financial reporting and technician payouts become ambiguous or wrong the moment the platform launches in a second country, and retrofitting currency onto a live payments table is one of the riskier categories of production migration.
Recommended solution: Add a Currency column (ISO 4217, char(3)) to Payments now, even if every row currently holds the same value.
F-11 — CreatedBy / LastModifiedBy stored as free text rather than a user reference
Severity	Medium
Category	Maintainability

Description: Every table's audit columns (CreatedBy, LastModifiedBy) are nullable text rather than a Guid foreign key to Users.Id.
Why it is a problem: Free-text audit columns cannot be joined back to the Users table to show "who did this" in an admin UI, cannot be validated, and are prone to inconsistent values (a display name in one place, an email in another, a system/service name in a third) as different parts of the codebase populate them differently over time.
Real-world impact: Audit/compliance reporting and "who changed this record" investigations become unreliable exactly when they matter most (disputes, fraud investigations, chargebacks).
Recommended solution: Change CreatedBy/LastModifiedBy to a nullable Guid FK to Users.Id (allowing NULL for system/background-job writes), or keep a text column specifically reserved for a well-defined set of system actor names and add a separate, FK-backed "ActorUserId" for human actions.
F-12 — No optimistic concurrency token on mutable, high-contention entities
Severity	Medium
Category	Performance / Integrity

Description: Assignments, ServiceRequests and Payments all represent state machines that can be updated concurrently (e.g. a technician accepting an assignment at the same moment it is auto-cancelled by a timeout job), but none of them carry a concurrency token.
Why it is a problem: Without a concurrency check, EF Core's last-write-wins behavior means two concurrent updates can silently overwrite each other's changes — for example a Status transition from a background timeout job racing a technician's "Accept" tap.
Real-world impact: Lost updates on Assignment/Payment status are the kind of bug that shows up as "the technician says they accepted but the system shows rejected" support tickets, and are very hard to reproduce after the fact.
Recommended solution: Map PostgreSQL's built-in xmin system column as an EF Core concurrency token (IsRowVersion) on Assignments, ServiceRequests and Payments — no schema/storage cost, since the column already exists on every Postgres table.
F-13 — Inconsistent soft-delete / enablement convention
Severity	Low
Category	Design

Description: Users uses a nullable DeletedAt column for soft delete, while Roles, ServiceCategories and others use an IsActive boolean instead. No other transactional entity (CustomerProfiles, TechnicianProfiles, ServiceRequests, etc.) has any deactivation/soft-delete column at all.
Why it is a problem: Two different conventions for "this row is no longer valid" force every future query and every future developer to remember which pattern a given table uses, which is an easy source of bugs (e.g. a report that filters IsActive = true on Users, which does not exist there).
Real-world impact: Increases onboarding time for new engineers and raises the chance of a query that accidentally includes soft-deleted or deactivated rows.
Recommended solution: Standardize on one convention per semantic meaning: DeletedAt for "this row should no longer exist" (customer/technician accounts, requests), and IsActive/IsEnabled strictly for "togglable configuration" (Roles, ServiceCategories). Document the convention in the architecture guide.
F-14 — Oversized varchar lengths on hashed columns
Severity	Low
Category	PostgreSQL

Description: RefreshTokens.RefreshTokenHash is character varying(512) and OtpRecords.CodeHash is character varying(256). Standard hashing algorithms (SHA-256 hex = 64 chars, SHA-512 hex = 128 chars, bcrypt = 60 chars) fit comfortably within a much smaller column.
Why it is a problem: Oversized varchar columns are not a correctness issue in PostgreSQL (storage is based on actual content, not declared max length), but they remove a useful, self-documenting guardrail against an application bug that accidentally stores something far larger than a hash (e.g. an unhashed JWT or a stack trace).
Real-world impact: Low direct impact, but a mismatched column length is often the first symptom that surfaces when a hashing algorithm is swapped, and a tighter length would fail fast instead of silently storing unexpected data.
Recommended solution: Right-size the columns to the actual hash algorithm's output length (e.g. varchar(64) for SHA-256 hex, or bytea if storing raw hash bytes instead of a hex/base64 string).
F-15 — UserRoles unique index blocks legitimate role re-assignment after revocation
Severity	Medium
Category	Integrity

Description: UserRoles has both a RevokedAt/IsActive soft-revocation pattern and a unique index on (UserId, RoleId). Because revoking a role only sets RevokedAt/IsActive rather than deleting the row, re-granting the same role to the same user later requires inserting a second (UserId, RoleId) row — which the unique index will reject.
Why it is a problem: The schema combines two conflicting patterns: an audit-friendly "never delete, just revoke" model and a hard uniqueness constraint that assumes only one row per (UserId, RoleId) ever exists. As written, once a role is revoked it can never be re-granted through a simple insert.
Real-world impact: Support staff or admin tooling attempting to restore a previously revoked role (e.g. reinstating a technician after a review) will hit constraint violations in production, and the workaround (deleting the old row) destroys the audit trail the RevokedAt/RevokedByUserId columns exist to preserve.
Recommended solution: Replace the plain unique index with a partial unique index scoped to active assignments only: UNIQUE (UserId, RoleId) WHERE IsActive = true. This preserves history while still preventing duplicate active role grants.
Example:
CREATE UNIQUE INDEX "IX_UserRoles_UserId_RoleId_Active" ON "UserRoles" ("UserId", "RoleId") WHERE "IsActive" = true;
F-16 — Uniform Restrict delete behavior applied even to dependent-only child tables
Severity	Medium
Category	EF Core

Description: Every single foreign key in the migration uses ReferentialAction.Restrict, including tables like OtpRecords, RefreshTokens, UserSessions, ServiceRequestImages and ServiceRequestTimelines, whose rows have no meaning independent of their parent (a ServiceRequestImage cannot exist without its ServiceRequest; an expired OtpRecord has no standalone value).
Why it is a problem: Restrict is a reasonable default for the schema's primary business entities (Users, ServiceRequests, Assignments) where accidental cascade deletion would be catastrophic. But applying it uniformly to purely dependent, high-churn tables means any cleanup job (e.g. purging expired OTPs or old sessions) or any legitimate cascading removal must manually delete every child table in the correct order rather than relying on the database to do it safely.
Real-world impact: Increases the amount of hand-written, order-sensitive cleanup code in the application/background-job layer, and raises the risk that a future cleanup script forgets one of the dependent tables and leaves orphaned Restrict-protected rows behind.
Recommended solution: Reserve Restrict for entities that represent independent business records (Users, ServiceRequests, Payments, Assignments) and use Cascade for tables that are pure children with no independent lifecycle (OtpRecords, ServiceRequestImages, ServiceRequestTimelines). RefreshTokens/UserSessions can reasonably go either way depending on whether you want token history retained after a user is removed.
F-17 — Sensitive PII stored without visible data classification or retention policy
Severity	Low
Category	Security

Description: NationalIdImageKey (a reference to a government ID image), precise Latitude/Longitude, FullAddress, and UserSessions.IpAddress/UserAgent are all stored as ordinary columns alongside operational data, with no schema-level indication of encryption-at-rest strategy, field-level access restriction, or retention/expiry policy.
Why it is a problem: This is exactly the category of data that attracts regulatory scrutiny (GDPR-style "right to erasure," data minimization, and breach-notification obligations) once the platform expands to multiple countries, and the schema currently gives no structural signal about which columns are considered sensitive.
Real-world impact: Absent an explicit retention policy, IP/UserAgent history and precise home-address coordinates accumulate indefinitely, increasing both storage cost and the blast radius of any future data breach or compliance audit.
Recommended solution: This migration alone cannot fully solve data classification, but the review should be paired with an explicit column-level sensitivity inventory, and consideration of a scheduled purge (e.g. UserSessions older than N days) and/or column-level encryption for NationalIdImageKey.
F-18 — Single default schema ahead of planned microservice decomposition
Severity	Low
Category	Scalability

Description: All 17 tables live in PostgreSQL's default public schema. The project's stated future direction includes decomposing into microservices (notifications, payments, analytics/reporting).
Why it is a problem: A flat public schema works fine at the current single-service stage, but makes it harder later to reason about ownership boundaries, apply per-domain permissions, or eventually split a schema into its own database/service without a disruptive rename/migration exercise.
Real-world impact: Not an immediate production risk, but a missed opportunity to make the eventual services split meaningfully cheaper.
Recommended solution: Consider grouping tables into logical PostgreSQL schemas today (e.g. identity, marketplace, billing) even while they remain in one database and one EF Core DbContext — this costs little now and pays off when boundaries are extracted later.
F-19 — No partitioning strategy for high-volume, time-ordered tables
Severity	Medium
Category	Performance

Description: RefreshTokens, OtpRecords, UserSessions and ServiceRequestTimelines are all naturally time-ordered, high-churn, and mostly queried by recency or by a short-lived ExpiresAt window, but the migration creates them as ordinary unpartitioned tables.
Why it is a problem: These are exactly the tables PostgreSQL's declarative partitioning (by CreatedAtUtc, monthly or weekly range partitions) is designed for: at 10M+ users, refresh tokens and OTPs alone can generate tens of millions of rows per month, and an unpartitioned table makes both routine cleanup (DELETE of expired rows) and VACUUM increasingly expensive over time.
Real-world impact: Without partitioning, purging expired tokens/OTPs turns into a slow, lock-heavy DELETE against an ever-growing table, and query planning for "recent sessions" style queries gets progressively slower as old, cold data accumulates in the same table as hot recent data.
Recommended solution: This does not need to be solved on day one, but it should be planned for before these tables reach tens of millions of rows: convert to a partitioned table (range-partitioned by CreatedAtUtc) so that expired partitions can be dropped instantly instead of deleted row-by-row.
F-20 — Payments table missing external gateway reference / idempotency key
Severity	Low
Category	PostgreSQL

Description: Payments has no column for a payment gateway's transaction/reference ID and no idempotency key to correlate a payment attempt with a specific client-initiated request.
Why it is a problem: Without a gateway reference, reconciling this table against the actual payment processor's records (required for any real payments integration) has no reliable join key. Without an idempotency key, a retried client request (e.g. a mobile app resubmitting after a timeout) has no database-level guard against creating a duplicate Payments row for the same charge.
Real-world impact: Duplicate-charge risk and painful manual reconciliation with the payment gateway are both realistic outcomes once real money is flowing through this table.
Recommended solution: Add a GatewayTransactionReference (nullable, populated once the gateway responds) and an IdempotencyKey column with a unique index, generated client-side per payment attempt.


3. Positive Design Decisions
The following aspects of the schema are well designed and should be preserved as the schema evolves.
✓ Consistent audit trail on every table
CreatedAtUtc, CreatedBy, LastModifiedUtc and LastModifiedBy appear on all 17 tables, with server-side CURRENT_TIMESTAMP defaults. This gives reliable, tamper-resistant-by-default traceability across the whole schema without relying on application code to remember to set timestamps.
✓ Correct use of timestamp with time zone
Every date/time column uses PostgreSQL's timestamptz rather than a naive timestamp, which is the right choice for a system that will operate across multiple countries and time zones.
✓ Profile-extension pattern instead of a bloated Users table
Splitting CustomerProfiles and TechnicianProfiles out from Users (rather than adding role-specific columns directly to Users) is a clean, DDD-aligned modeling choice that keeps the core identity table lean and lets each role evolve independently.
✓ Hashed storage of secrets
Both RefreshTokens.RefreshTokenHash and OtpRecords.CodeHash store a hash rather than a plaintext value, correctly following the principle of never persisting recoverable authentication secrets.
✓ Well-designed RBAC audit trail on UserRoles
AssignedByUserId, RevokedAt and RevokedByUserId turn a simple many-to-many join table into a proper access-audit record of who granted/revoked which role and when — this is more thorough than many production schemas at this stage.
✓ TechnicianServices composite unique index
The unique index on (TechnicianProfileId, ServiceCategoryId) correctly prevents a technician from being assigned to the same service category twice — a domain rule enforced at the database layer rather than trusted to application code.
✓ ServiceRequestTimelines as an explicit event/history table
Rather than only tracking current Status on ServiceRequests, a dedicated timeline table preserves the full history of status transitions — valuable for both customer-facing tracking UIs and future analytics/reporting.
✓ Sensible numeric precision for money and coordinates
Payments.Amount as numeric(12,2) and Latitude/Longitude as numeric(9,6) both use appropriate, deliberate precision rather than floating-point types, avoiding the classic rounding-error pitfalls of using float/double for money or coordinates.
✓ Consistent naming and FK conventions
Table names, PK names (PK_{Table}), and FK names (FK_{Table}_{PrincipalTable}_{Column}) follow a single, predictable convention throughout, which materially reduces onboarding time and makes the generated migration easy to diff and review.
✓ UUID primary keys support distributed ID generation
Using uuid rather than an auto-incrementing integer means IDs can be generated client-side or by any future microservice without a round trip to a central sequence — the right instinct for the planned microservices future, even though the specific UUID version needs revisiting (see F-05).
4. Scalability Review — Readiness for 10M+ Users
Authentication & Session Data
RefreshTokens, OtpRecords and UserSessions are correctly separated from Users, which is good — but as high-churn, short-lived tables they are the first to feel the effects of random UUID primary keys (F-05) and the lack of a partitioning strategy (F-19). These tables should be the first candidates for optimization since they will have the highest write volume of any tables in the system.
Geographic & Multi-Country Expansion
Addresses currently has no reference tables for country/city/area and no geospatial indexing (F-09), and Payments has no currency column (F-10). Both are low-cost to add now and high-cost to retrofit once the platform has live data in a second country — these should be prioritized ahead of any international launch, not after.
Matching & Dispatch Queries
The core marketplace loop — matching an open ServiceRequest to an available TechnicianProfile — is currently underserved by the index set (F-06) and by the lack of geospatial querying (F-09). This is the schema's most latency-sensitive workload and deserves index and query-pattern review before load testing.
Analytics & Reporting
The schema has no dedicated reporting/read-replica considerations, which is expected at this stage, but the plain-integer enum columns (F-08) will make any future BI/reporting tool (including a data warehouse or analytics microservice) harder to interpret without joining back to application-level enum definitions. Promoting frequently-reported-on enums to lookup tables now will pay off significantly once reporting requirements arrive.
Microservice Decomposition
The schema's tables map cleanly to plausible future service boundaries (identity/auth, marketplace, payments), which is a good sign. Schema namespacing (F-18) is a low-cost step that would make that eventual decomposition meaningfully cheaper.
5. Final Score
Dimension	Score
Database Design	6.5 / 10
Relationships	5.0 / 10
Indexing	5.0 / 10
Constraints	4.0 / 10
Security	6.0 / 10
Performance	5.5 / 10
Maintainability	7.0 / 10
Scalability	5.0 / 10
Overall Score	5.5 / 10

The overall score of 5.5/10 reflects a schema with a genuinely solid structural foundation — consistent conventions, correct core relationships, and sound choices on data types — that is undermined by an incomplete integrity layer (no CHECK constraints, missing uniqueness on identity and profile fields) and a handful of PostgreSQL-specific performance considerations that matter specifically because of this project's stated scale target. Addressing the Critical and High findings (F-01 through F-06) before launch would move this schema to a 7.5–8/10 with comparatively little engineering effort.