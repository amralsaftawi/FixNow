# Chapter 04 — System Overview

> *"Before understanding the implementation, we must understand the system as a whole."*

---

# Introduction

Before diving into the architecture and implementation details, it is important to understand the **big picture**.

FixNow is composed of multiple independent layers and business components that collaborate to provide a complete home services marketplace.

This chapter introduces the overall structure of the system and the responsibilities of each major component.

---

# High-Level System

```mermaid
flowchart LR

Customer["Customer"]
Technician["Technician"]
Admin["Administrator"]

API["ASP.NET Core API"]

Application["Application Layer"]

Domain["Domain Layer"]

Infrastructure["Infrastructure Layer"]

Database["PostgreSQL"]

Storage["Object Storage"]

Customer --> API
Technician --> API
Admin --> API

API --> Application

Application --> Domain

Application --> Infrastructure

Infrastructure --> Database

Infrastructure --> Storage
```

---

# System Layers

The application follows **Clean Architecture**.

```mermaid
flowchart TB

Presentation["Presentation (API)"]

Application["Application Layer"]

Domain["Domain Layer"]

Infrastructure["Infrastructure Layer"]

Presentation --> Application

Application --> Domain

Application --> Infrastructure
```

Each layer has a single responsibility.

---

# Core Business Flow

The platform revolves around a simple but powerful workflow.

```mermaid
flowchart LR

Customer --> Request["Create Service Request"]

Request --> Assignment["Assign Technician"]

Assignment --> Service["Service Execution"]

Service --> Payment["Payment"]

Payment --> Review["Review"]
```

Everything inside the project ultimately supports this workflow.

---

# Core Domain Model

The following diagram shows the primary business entities.

```mermaid
flowchart LR

User

CustomerProfile

TechnicianProfile

ServiceCategory

TechnicianService

ServiceRequest

Assignment

Payment

Review

User --> CustomerProfile

User --> TechnicianProfile

TechnicianProfile --> TechnicianService

TechnicianService --> ServiceCategory

CustomerProfile --> ServiceRequest

ServiceRequest --> Assignment

Assignment --> Payment

Assignment --> Review
```

---

# Request Lifecycle

A service request goes through several states during its lifetime.

```mermaid
stateDiagram-v2

[*] --> Draft

Draft --> Searching

Searching --> Assigned

Assigned --> InProgress

InProgress --> Completed

Searching --> Cancelled

Assigned --> Cancelled
```

---

# Assignment Lifecycle

```mermaid
stateDiagram-v2

[*] --> Pending

Pending --> Accepted

Pending --> Rejected

Accepted --> Completed
```

---

# Payment Lifecycle

```mermaid
stateDiagram-v2

[*] --> Pending

Pending --> Paid

Pending --> Failed

Paid --> Refunded
```

---

# Review Lifecycle

```mermaid
flowchart LR

CompletedAssignment --> ReviewCreated

ReviewCreated --> ReviewUpdated
```

---

# Bounded Contexts

Although the MVP is implemented as a modular monolith, the business domain is organized into logical bounded contexts.

| Context         | Responsibility                           |
| --------------- | ---------------------------------------- |
| Identity        | Users and authentication                 |
| Customer        | Customer profiles and addresses          |
| Technician      | Technician profiles and offered services |
| Service Catalog | Available service categories             |
| Service Request | Customer requests and tracking           |
| Assignment      | Technician assignment workflow           |
| Payment         | Payment processing                       |
| Review          | Customer feedback                        |

These contexts reduce coupling and make future extraction into microservices possible.

---

# Design Principles

Several architectural principles guide the implementation.

* Business logic belongs to the Domain Layer.
* Application Layer orchestrates use cases.
* Infrastructure contains technical details.
* API only exposes the application.
* Dependencies always point inward.
* Rich Domain Model over Anemic Domain Model.

---

# Why This Architecture?

This architecture was selected because it provides:

* Clear separation of responsibilities.
* High maintainability.
* Excellent testability.
* Long-term scalability.
* Easy onboarding for new developers.
* Independence from frameworks and databases.

---

# Summary

At this point, we have a high-level understanding of the system.

We know:

* The major actors.
* The overall workflow.
* The primary business entities.
* The lifecycle of important business processes.
* The responsibilities of each architectural layer.

The next chapter explains **why Clean Architecture was selected** and how it serves as the foundation of the entire project.

---

# Next Chapter

➡️ **Chapter 05 — Clean Architecture**
