# Nexa App – Digital Wallet API

🚧 Status: Under Active Development

⚠️ Not production ready


> Nexa App is a modular digital wallet application built using Domain-Driven Design and event-driven architecture to enable scalable, secure, and extensible financial operations.

## 📖 Overview

**Nexa App** is a modular digital wallet backend designed using Domain-Driven Design (DDD) and event-driven architecture principles.

It provides a secure and scalable foundation for managing wallets, processing financial transactions, and integrating with external payment systems through webhooks and asynchronous events.

### 🎯 Core Responsibilities

- Wallet lifecycle management
- Transaction processing and tracking
- Balance consistency enforcement
- Event-based financial processing
- External payment integration via webhooks
- Extensible domain modules for future financial services

### 🏗 Architectural Principles

- Modular monolithic structure with bounded contexts
- Domain-Driven Design for business logic isolation
- Event-driven communication for transaction processing
- Clean separation between Domain, Application, and Infrastructure layers

The system is built to remain maintainable while supporting growth into a distributed or microservice-based future if required.

## ✨ Features

### 👤 Customer Management
- Customer profile creation and management
- Identity linking to wallet accounts
- Profile updates and status tracking
- Manage a single address per customer (create, update, delete)
- Enforce 1:1 relationship between customer and address

### 🔐 KYC (Know Your Customer)
- KYC verification workflow
- Document upload & validation
- Identity verification status tracking

### 💳 Wallet Management
- Create and manage user wallets
- Track wallet balances
- Support multi-account wallet structure
- Wallet status management (active, frozen)

### 💰 Transaction Processing

Transactions are processed asynchronously with state tracking and event-driven execution.

#### 🔄 P2P Transactions
- Wallet-to-wallet transfers
- Async processing with status tracking (Pending → Completed / Failed)
- Atomic balance validation
- Idempotency handling
- Transaction event publishing

#### 🏦 Bank Transactions
- Bank deposits and withdrawals
- Async processing via external banking integration
- Pending state until bank confirmation
- Webhook-based status updates
- Failure handling and compensation logic
- External reference tracking

All transactions include:
- Status tracking
- Failure recovery handling
- Event-based state transitions

### 🔄 Event-Driven Processing
- Domain events for financial operations
- Asynchronous event handling
- Webhook-based external integrations
- Event persistence & audit tracking

### 🔐 Security
- JWT-based authentication
- Transaction validation rules
- Webhook signature verification

### 🔌 External Integrations
- Payment gateway webhooks
- External service event consumption
- Extensible integration layer

### 🧪 Developer Experience
- Swagger API documentation
- Environment-based configuration
- Modular architecture for easy extension

## 🏗 Architecture

Nexa App follows a modular architecture based on Domain-Driven Design (DDD) and event-driven principles to ensure scalability and maintainability.

### 🔷 Architectural Principles
- Modular monolith with clearly defined bounded contexts
- Domain-Driven Design for business logic isolation
- Event-driven communication between modules
- Asynchronous transaction processing
- Clean separation of concerns
