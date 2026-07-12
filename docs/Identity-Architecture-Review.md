# Identity Domain Model

## Overview

This diagram illustrates the Identity bounded context domain model, including the aggregate root, entities, and value objects.

```mermaid
classDiagram
direction TB

class User {
    +Guid Id
    +FullName
    +Email
    +PhoneNumber
    +PasswordHash
    +CountryCode
    +AccountStatus
    +Register()
    +Activate()
    +Deactivate()
    +ChangePassword()
    +AssignRole()
    +RevokeRole()
}

class UserRole {
    +Guid UserId
    +Guid RoleId
    +AssignedAt
}

class Role {
    +Guid Id
    +Name
    +Description
}

class RefreshToken {
    +Guid Id
    +Token
    +ExpiresAt
    +Revoke()
}

class UserSession {
    +Guid Id
    +DeviceName
    +IpAddress
    +LastActivity
}

class OTPRecord {
    +Guid Id
    +Code
    +Purpose
    +ExpiresAt
}

class Email
class PhoneNumber
class PasswordHash
class CountryCode

User "1" --> "*" UserRole
Role "1" --> "*" UserRole

User "1" --> "*" RefreshToken
User "1" --> "*" UserSession
User "1" --> "*" OTPRecord

User *-- Email
User *-- PhoneNumber
User *-- PasswordHash
User *-- CountryCode
```

---

## Aggregate Root

**User** is the aggregate root of the Identity bounded context.

All identity-related operations must go through the `User` aggregate.

---

## Notes

- `UserRole` represents role assignments.
- `RefreshToken` manages authentication sessions.
- `UserSession` tracks active user sessions.
- `OTPRecord` stores one-time password verification records.
- `Email`, `PhoneNumber`, `PasswordHash`, and `CountryCode` are immutable Value Objects.