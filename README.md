# Nexa App – Digital Wallet API

🚧 Status: Under Active Development

⚠️ Not production ready


> Nexa App is a modular digital wallet application built using Domain-Driven Design and event-driven architecture to enable scalable, secure, and extensible financial operations.

## 📖 Overview

**Nexa App** is a modular digital wallet backend designed using Domain-Driven Design (DDD) and event-driven architecture principles.

It provides a secure and scalable foundation for managing wallets, processing financial transactions, and integrating with external payment systems through webhooks and asynchronous events.



## ✨ Features

#### 👤 Customer Management

#### 🔐 KYC (Know Your Customer)

#### 💳 Wallet Management

#### 💰 Transaction Processing

#### 🔄 P2P Transactions

#### 🏦 Bank Transactions

#### 🔄 Event-Driven Processing

#### 🔐 Security


## 🔷 Architectural Principles
- Modular monolith with clearly defined bounded contexts
- Domain-Driven Design for business logic isolation
- Event-driven communication between modules
- Asynchronous transaction processing
- Clean separation of concerns

## 🚀 Getting Started

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
   cd Nexa ```

2. **Install .NET 8 SDK** 
     Verify installation:
     ```bash
      dotnet --version ```

3. **Restore Dependencies**
   ```bash
     dotnet restore```
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
        ngrok http [APPLICATION_PORT]
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

