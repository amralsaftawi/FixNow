# Chapter 02 — Business Problem

> *"Software exists to solve business problems—not technical ones."*

---

# Introduction

Before discussing software architecture, design patterns, or technologies, we must first understand **the business problem**.

Every architectural decision in FixNow originates from a real-world problem faced by customers, technicians, and service providers.

Architecture is a consequence of business requirements.

---

# The Problem

Finding a trustworthy technician for home maintenance is still a frustrating experience in many regions.

When a customer faces a problem such as:

* A leaking pipe
* A power outage
* A broken air conditioner
* Damaged furniture
* A blocked drain

the typical process looks like this:

1. Ask friends or family.
2. Search Facebook groups.
3. Call random phone numbers.
4. Wait for someone to answer.
5. Negotiate the price.
6. Hope the technician actually arrives.

This process is:

* Slow
* Unreliable
* Difficult to track
* Lacking transparency
* Based on trust rather than verification

---

# Customer Pain Points

Customers often experience problems such as:

* Difficulty finding available technicians.
* No guarantee of technician quality.
* Unknown arrival times.
* No real-time tracking.
* Unclear pricing.
* Poor communication.
* No review or rating system.
* No service history.

As a result, customers waste time before the actual repair even begins.

---

# Technician Pain Points

Technicians also face challenges.

Many skilled technicians:

* Depend entirely on word-of-mouth.
* Have inconsistent income.
* Receive requests through phone calls only.
* Have no digital profile.
* Cannot easily build their professional reputation.
* Cannot manage customer requests efficiently.

Finding work becomes unpredictable despite having valuable skills.

---

# Market Opportunity

Both sides share the same problem.

Customers struggle to find trusted technicians.

Technicians struggle to find customers.

FixNow acts as the bridge between both parties.

---

# Proposed Solution

FixNow provides a centralized platform where:

Customers can:

* Request services within minutes.
* Describe the problem.
* Upload images.
* Track technicians in real time.
* Pay securely.
* Rate completed services.

Technicians can:

* Build professional profiles.
* Verify their identity.
* Manage incoming requests.
* Accept or reject assignments.
* Track earnings.
* Build a public reputation through reviews.

---

# Business Goals

The primary business objectives are:

* Reduce customer waiting time.
* Increase customer trust.
* Improve technician visibility.
* Digitize home service requests.
* Standardize the service workflow.
* Increase service quality through accountability.
* Create a scalable marketplace.

---

# High-Level Workflow

```text
Customer
    │
    ▼
Create Service Request
    │
    ▼
Find Available Technician
    │
    ▼
Technician Accepts
    │
    ▼
Service Execution
    │
    ▼
Payment
    │
    ▼
Review & Rating
```

---

# Core Actors

The platform revolves around four primary actors.

## Customer

Requests home services and evaluates completed work.

---

## Technician

Provides home services after identity verification.

---

## Administrator

Manages categories, technicians, disputes, and platform operations.

---

## System

Coordinates matching, payments, notifications, and workflow automation.

---

# Functional Scope (MVP)

The first version of FixNow focuses on the essential business capabilities.

### Identity

* User registration
* Login
* OTP verification

### Customer

* Manage profile
* Manage addresses
* Request services

### Technician

* Manage profile
* Verification
* Service categories
* Availability

### Service Requests

* Create requests
* Upload images
* Assign technicians
* Track progress

### Assignment

* Accept or reject jobs
* Complete services

### Payments

* Record payments
* Refunds
* Payment status tracking

### Reviews

* Customer ratings
* Technician feedback

---

# Non-Functional Goals

The platform is designed to achieve:

* Scalability
* Maintainability
* Testability
* Security
* Extensibility
* High cohesion
* Low coupling
* Clear separation of responsibilities

These goals directly influence every architectural decision discussed later in this book.

---

# Why This Matters

Understanding the business problem is essential before discussing software architecture.

Every Aggregate, Entity, Value Object, and Domain Event that appears later in this book exists because it solves one or more of the problems described in this chapter.

The Domain Model is therefore not driven by technology—it is driven by business.

---

# Next Chapter

➡️ **Chapter 03 — Project Vision**

Now that we understand the business problem, we can define the long-term vision of FixNow and the principles that guide its evolution into a scalable enterprise platform.
