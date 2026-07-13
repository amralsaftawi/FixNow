# Chapter 01 — Introduction

> *"Every great software system starts with a real business problem, not with code."*

---

# Welcome to the FixNow Architecture Book

Welcome!

This book documents the complete architecture, design decisions, and implementation strategy behind **FixNow**, a modern on-demand home services platform built using **Domain-Driven Design (DDD)**, **Clean Architecture**, **Vertical Slice Architecture**, and **CQRS**.

Unlike traditional documentation that focuses only on explaining the code, this book explains the **thinking process behind every architectural decision**.

It answers questions such as:

* Why was this entity created?
* Why is this class an Aggregate Root?
* Why is this object a Value Object instead of an Entity?
* Why was this business rule placed inside the Domain Layer?
* Why was a particular design rejected?

This book aims to document not only *what* was built, but also *why* it was built this way.

---

# About FixNow

FixNow is an on-demand home services platform that connects customers with verified technicians to solve home maintenance and emergency repair problems.

The platform allows customers to:

* Request home maintenance services.
* Upload images describing the problem.
* Receive technician assignments.
* Track service progress.
* Complete secure payments.
* Rate and review technicians.

Supported services include:

* Plumbing
* Electrical Repairs
* Air Conditioning
* Carpentry
* Cleaning
* Additional home services

---

# Why This Book Exists

Enterprise software is much more than writing code.

A well-designed system should also document:

* Business requirements.
* Architectural decisions.
* Design trade-offs.
* Domain knowledge.
* System evolution.

Without proper documentation, future developers often spend more time understanding the system than improving it.

This book solves that problem by acting as the official architectural reference for the entire project.

---

# Target Audience

This book is written for:

* Backend Developers
* Software Engineers
* Software Architects
* Computer Science Students
* Developers learning Domain-Driven Design
* Anyone interested in enterprise software architecture

No prior knowledge of FixNow is required.

---

# What Makes This Documentation Different?

Most project documentation explains **how** the system works.

This book explains:

* **Why** every decision was made.
* **How** business requirements shaped the architecture.
* **What** alternatives were considered.
* **Which** trade-offs were accepted.

Every important architectural decision is documented using Architecture Decision Records (ADRs), making the reasoning behind the project transparent.

---

# Project Philosophy

The project follows a few fundamental principles:

* Business logic belongs inside the Domain Layer.
* Rich Domain Model over Anemic Domain Model.
* Explicit business rules.
* Small, focused aggregates.
* High cohesion and low coupling.
* Clear separation of concerns.
* Architecture should support future scalability.

These principles influence every line of code written in the project.

---

# Book Structure

The book is organized into multiple chapters, each focusing on a specific aspect of the system.

1. Introduction
2. Business Problem
3. Project Vision
4. System Overview
5. Clean Architecture
6. Domain-Driven Design
7. Domain Layer
8. Application Layer
9. Infrastructure Layer
10. API Layer
11. Testing
12. Deployment
13. Future Roadmap

Each chapter builds on the previous one, so reading them in order is highly recommended.

---

# Learning Objectives

By the end of this book, you should understand:

* How to design a real-world enterprise application.
* How Domain-Driven Design is applied in practice.
* How Clean Architecture separates responsibilities.
* How Vertical Slice Architecture organizes features.
* How business rules are modeled using Aggregates and Value Objects.
* How architectural decisions affect maintainability and scalability.

---

# A Note Before We Begin

This is not a theoretical book.

Everything discussed throughout these chapters is based on a real project.

Every Aggregate, Value Object, Domain Event, Command, Query, and API endpoint presented in this documentation is part of the actual FixNow codebase.

The goal is to bridge the gap between software architecture theory and production-ready implementation.

---

# Next Chapter

➡️ **Chapter 02 — Business Problem**

Before discussing architecture, we must first understand the business problem that FixNow was created to solve.

As with any successful software system, architecture is a consequence of business needs—not the other way around.
