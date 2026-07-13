# Domain Events Flow

## Overview

This document illustrates how aggregates communicate through domain events.

Rather than directly modifying other aggregates, each aggregate raises domain events that are handled by the Application Layer or dedicated event handlers.

This approach keeps aggregate boundaries clean, reduces coupling, and supports future scalability.

---

# High-Level Event Flow

```mermaid
flowchart LR

User

CustomerProfile

TechnicianProfile

ServiceRequest

Assignment

Payment

Review

User -->|UserRegistered| CustomerProfile

User -->|UserRegistered| TechnicianProfile

CustomerProfile -->|CustomerAddressAdded| ServiceRequest

ServiceRequest -->|ServiceRequestCreated| Assignment

Assignment -->|AssignmentCompleted| Payment

Assignment -->|AssignmentCompleted| Review
```

---

# Registration Flow

```mermaid
sequenceDiagram

participant User
participant DomainEvents
participant Application

User->>DomainEvents: UserRegistered

DomainEvents->>Application: Handle Event

Application->>CustomerProfile: Create()

Application->>TechnicianProfile: Create()
```

A newly registered user may later become a customer or technician.

The domain event decouples the User aggregate from business profile creation.

---

# Service Request Flow

```mermaid
sequenceDiagram

participant Customer
participant ServiceRequest
participant DomainEvents
participant Application

Customer->>ServiceRequest: Create()

ServiceRequest-->>DomainEvents: ServiceRequestCreated

DomainEvents->>Application: Handle Event

Application->>Assignment: Start Matching
```

---

# Assignment Completion Flow

```mermaid
sequenceDiagram

participant Technician
participant Assignment
participant DomainEvents
participant Payment
participant Review

Technician->>Assignment: Complete()

Assignment-->>DomainEvents: AssignmentCompleted

DomainEvents->>Payment: Create Payment

DomainEvents->>Review: Customer Can Review
```

---

# Payment Flow

```mermaid
sequenceDiagram

participant Payment
participant DomainEvents
participant Notification

Payment->>DomainEvents: PaymentSucceeded

DomainEvents->>Notification: Send Receipt

Payment->>DomainEvents: PaymentRefunded

DomainEvents->>Notification: Notify Customer
```

---

# Review Flow

```mermaid
sequenceDiagram

participant Customer

participant Review

participant DomainEvents

Review->>DomainEvents: ReviewCreated

Review->>DomainEvents: ReviewUpdated
```

---

# Event Propagation

```text
Aggregate

      │

      ▼

Business Action

      │

      ▼

Domain Event

      │

      ▼

Application Layer

      │

      ▼

Event Handler

      │

      ▼

Other Aggregate
```

No aggregate directly modifies another aggregate.

Communication always occurs through events and application services.

---

# Event Ownership

| Aggregate | Example Domain Events |
|------------|----------------------|
| User | UserRegistered, UserActivated, UserSuspended |
| CustomerProfile | CustomerProfileCreated, CustomerAddressAdded |
| Address | AddressCreated, AddressUpdated |
| TechnicianProfile | TechnicianVerified, TechnicianProfileCompleted |
| ServiceCategory | ServiceCategoryCreated |
| ServiceRequest | ServiceRequestCreated, Scheduled, Completed, Cancelled |
| Assignment | AssignmentCreated, Accepted, Rejected, Completed |
| Payment | PaymentCreated, PaymentSucceeded, PaymentRefunded |
| Review | ReviewCreated, ReviewUpdated |

---

# Event Lifecycle

```mermaid
flowchart TD

Business Action

↓

Aggregate

↓

Validate Business Rules

↓

Update Aggregate State

↓

Raise Domain Event

↓

Commit Transaction

↓

Publish Event

↓

Execute Event Handlers
```

---

# Design Principles

- Aggregates never call each other directly.
- Business actions produce domain events.
- Domain events represent facts that already happened.
- Event handlers coordinate cross-aggregate workflows.
- Aggregates remain independent and focused on their own invariants.
- The Application Layer is responsible for orchestrating event processing.