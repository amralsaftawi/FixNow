# 📘 FixNow Architecture Book

> **Building a Modern On-Demand Home Services Platform using Domain-Driven Design, Clean Architecture, Vertical Slice Architecture, CQRS, and ASP.NET Core.**

---

## Welcome 👋

Welcome to the **FixNow Architecture Book**.

This documentation is not just a code reference.

It explains **why** every architectural decision was made, how the business domain was modeled, and how the entire system evolved from an idea into a production-ready enterprise application.

Whether you're:

* A Backend Developer
* A Software Architect
* A Student learning DDD
* A Recruiter reviewing the project

this book is designed to help you understand the system from both a **business** and **technical** perspective.

---

# What is FixNow?

FixNow is an on-demand home services platform that connects customers with verified technicians for maintenance and emergency repair services.

Examples include:

* Plumbing
* Electrical Repairs
* Air Conditioning
* Carpentry
* Cleaning Services

The platform allows customers to request services, track technicians in real time, complete secure payments, and leave reviews after each completed job.

---

# Documentation Roadmap

```
Business Problem
        │
        ▼
Project Vision
        │
        ▼
System Architecture
        │
        ▼
Domain Layer
        │
        ▼
Application Layer
        │
        ▼
Infrastructure Layer
        │
        ▼
API Layer
        │
        ▼
Testing
        │
        ▼
Deployment
```

---

# Architecture Overview

```
                    Client Apps
      ┌───────────────────────────────────┐
      │ Mobile App │ Web Dashboard │ Admin│
      └───────────────────────────────────┘
                     │
                     ▼
              ASP.NET Core API
                     │
 ┌─────────────────────────────────────────┐
 │         Application Layer               │
 │   Commands • Queries • Handlers         │
 └─────────────────────────────────────────┘
                     │
                     ▼
 ┌─────────────────────────────────────────┐
 │            Domain Layer                 │
 │ Business Rules • Aggregates • Events    │
 └─────────────────────────────────────────┘
                     │
                     ▼
 ┌─────────────────────────────────────────┐
 │        Infrastructure Layer             │
 │ PostgreSQL • EF Core • Storage • Cache  │
 └─────────────────────────────────────────┘
```

---

# Technology Stack

| Layer                | Technology                  |
| -------------------- | --------------------------- |
| Backend              | ASP.NET Core                |
| Language             | C#                          |
| Database             | PostgreSQL                  |
| ORM                  | EF Core                     |
| Architecture         | Clean Architecture          |
| Design               | Domain-Driven Design        |
| Feature Organization | Vertical Slice Architecture |
| Pattern              | CQRS                        |
| Result Pattern       | Functional Result Pattern   |
| Authentication       | JWT                         |
| Storage              | Cloud Object Storage        |

---

# Book Structure

| Chapter | Description          |
| ------- | -------------------- |
| 01      | Business Problem     |
| 02      | Project Vision       |
| 03      | Clean Architecture   |
| 04      | Domain-Driven Design |
| 05      | Domain Layer         |
| 06      | Application Layer    |
| 07      | Infrastructure Layer |
| 08      | API Layer            |
| 09      | Testing              |
| 10      | Deployment           |

---

# Goals of this Book

* Explain every architectural decision.
* Document the complete business domain.
* Serve as a reference for future contributors.
* Demonstrate real-world enterprise software engineering practices.
* Provide a learning resource for developers interested in DDD and Clean Architecture.

---

## Let's Begin →

Start with **Chapter 01 — Business Problem**.
