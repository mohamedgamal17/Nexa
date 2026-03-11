# Nexa App – Digital Wallet API

🚧 Status: Under Active Development

⚠️ Not production ready


> Nexa App is a modular digital wallet application built using Domain-Driven Design and event-driven architecture to enable scalable, secure, and extensible financial operations.

## 📖 Overview

**Nexa App** is a modular digital wallet backend designed using Domain-Driven Design (DDD) and event-driven architecture principles.

It provides a secure and scalable foundation for managing wallets, processing financial transactions, and integrating with external payment systems through webhooks and asynchronous events.

---
## Features

#### 👤 Customer Management

#### 🔐 KYC (Know Your Customer)

#### 💳 Wallet Management

#### 💰 Transaction Processing

#### 🔄 P2P Transactions

#### 🏦 Bank Transactions

#### 🔄 Event-Driven Processing

#### 🔐 Security

---
## Architecture
This project follows a **modular architecture** where the application is divided into independent modules.  
Each module encapsulates its own domain logic, data access, and API endpoints.
/*image support */
This approach improves:
- Maintainability
- Scalability
- Separation of concerns
- Independent feature development

### 📦 Modules
Each module represents a specific business capability and is isolated from other modules.

Application modules:

- **Cusomer Management Module**: Handles customer management, profiles, and kyc verification logic.

- **Accountant Module**: Manages customer wallets and their external bank accounts.
  
- **Transaction Module**: Manages all financial operations within the digital wallet system. It records every movement of funds to ensure transparency, traceability, and financial integrity.


Each modules follows Clean Architecture, separating concerns into distinct layers:
 - **Presentation** : Handles API requests
 - **Infrastructure** : Data persistence, external services, and message broker service
 - **Application** : Business logic and use cases with CQRS Pattern and Mediator Pattern
 - **Domain** :  Core entities, aggregates, and domain services
---
## Project structure
## 📂 Project Structure

  ### Project Modularity structure:
  ```bash
  📁 src/
  └── 📁 Modules/
      ├── 📁 Accoounting/
      ├── 📁 CustomerManagement/
      └── 📁 Transactions/
```
  ### Module structure
  ```bash
  📁 Acccounting/
  ├── 📁 Nexa.Accounting.Domain/
  ├── 📁 Nexa.Accounting.Application/
  ├── 📁 Nexa.Accounting.Infrastructure/
  └── 📁 Nexa.Accounting.Presentation/
  
  ```
  ### Test structure
  ```bash
   📁 tests/
    ├── 📁 Modules/
    │   ├── 📁 Accounting/
    │   │   └── 📁 Nexa.Accounting.Application.Tests/
    │   ├── 📁 CustomerManagement/
    │   │   └── 📁 Nexa.CustomerManagement.Application.Tests/
    │   └── 📁 Transactions/
    │       └── 📁 Nexa.Transactions.Application.Tests/
    └── 📁 Nexa.Application.Tests/
  ```
 



---

## Configuration

1. **Stripe Configuration**
  ```json
  "Baas": {
    "ApiKey": "YOUR_STRIPE_API_KEY",
    "WebhookSecret": "YOUR_STRIPE_WEBHOOK_SECRET",
    "FinancialAccounts": {
      "Main": "YOUR_STRIPE_MAIN_FINANCIAL_CONNECTION_ACCOUNT" 
    }
  }
  ```
2. **ComplyCube Configuration**
```json
"ComplyCube": {
    "ApiKey": "YOUR_COMPLYCUBE_API_KEY",
    "WebhookSecret": "YOUR_WEBHOOK_SECRET"
  }
```
3. **RabbitMQ Configuration**
```json
  "RabbitMq": {
    "Host": "RABBITMQ_HOST",
    "UserName": "RABBITMQ_USERNAME",
    "Password": "RABBITMQ_PASSWORD"
  }
```
4. **SQL Server Configuraiton**
```json
 "ConnectionStrings": {
    "Default": "YOUR_SQL_SERVER_DB_CONNECTION_STRING"
  }
```
---
## Getting Started

### Prerequisites

Ensure you have the following installed:
- **.NET 8 SDK**  [Download]( https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server**
- **IDE (optional but recommended)**
  - Visual Studio
  - Visual Studio Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/mohamedgamal17/Nexa.git
   cd Nexa
   ```

2. **Install .NET 8 SDK** 
     Verify installation:
     ```bash
      dotnet --version
     ```

3. **Restore Dependencies**
   ```bash
     dotnet restore
   ```
4. **Configure Application Settings**
```json
  "ConnectionStrings": {
    "Default": "YOUR_SQL_SERVER_DB_CONNECTION_STRING"
  },
  "RabbitMq": {
    "Host": "RABBITMQ_HOST",
    "UserName": "RABBITMQ_USERNAME",
    "Password": "RABBITMQ_PASSWORD"
  },
  "OpenBanking": {
    "ClientId": "YOUR_STRIPE_API_KEY",
    "ClientSecret": "YOUR_STRIPE_CLIENT_SECRET"
  },
  "Baas": {
    "ApiKey": "YOUR_STRIPE_API_KEY",
    "WebhookSecret": "YOUR_STRIPE_WEBHOOK_SECRET",
    "FinancialAccounts": {
      "Main": "YOUR_STRIPE_MAIN_FINANCIAL_CONNECTION_ACCOUNT" 
    }
  },
  "ComplyCube": {
    "ApiKey": "YOUR_COMPLYCUBE_API_KEY",
    "WebhookSecret": "YOUR_WEBHOOK_SECRET"
  }
```
5. **Apply Database Migrations**
  ```bash
    dotnet ef database update
  ```

6. **Ensure Webhooks are Reachable (Local Development)**
  To receive webhook events from Stripe or ComplyCube on your local machine: 
    1. Start a tunnel using ngrok:
      ```bash
        ngrok http [APPLICATION_URL]
      ```
    2. Copy the generated public URL (e.g., https://abc123.ngrok.io) and set it in your provider's webhook settings for Stripe or ComplyCube.
     
    3. Ensure your API endpoint for webhooks is exposed at:
       
       For stripe
       ```bash
       POST /api/webhooks/stripe
       ```
       For comblycube
        ```bash
       POST /api/webhooks/comblycube
       ```
7. **Build the Project**
  ```bash
dotnet build
  ```
8. **Run the Application**
```bash
dotnet run
```

The API will be available at:

   ```bash
     https://localhost:7226
   ```
---
## 🧪 Testing
The project uses integration tests to verify the behavior of the application as a whole. Tests focus on the Application Layer, ensuring that use cases, module interactions, and infrastructure integrations work correctly.

All tests run inside Testcontainers, which spin up the required dependencies (such as databases) in isolated Docker containers. This approach provides a realistic testing environment that closely matches production behavior.

### Requirements
  - Docker must be installed and running.
### Running Tests
Execute the following command from the root of the project:
```base
dotnet test
```
During test execution, the required containers will automatically start and be disposed after the tests complete.
