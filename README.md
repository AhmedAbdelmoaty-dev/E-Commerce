# E-Commerce Full Stack Application

A full-stack e-commerce platform built with **ASP.NET Core Web API** and **Angular**, following **Clean Architecture** and modern backend design patterns for scalability and maintainability.

## Tech Stack

### Backend
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- MediatR
- Redis

### Frontend
- Angular
- TypeScript
- RxJS

### Architecture & Patterns
- Clean Architecture
- CQRS
- Specification Pattern
- Repository Pattern

---

## Features

- User authentication and role-based authorization (JWT)
- Product browsing with filtering, searching, sorting, and pagination
- Persistent shopping cart using Redis
- Order creation and order history
- Stripe payment integration with secure checkout

---

## Architecture Overview

The solution is structured using **Clean Architecture**:

- **Domain**: Core business entities and rules  
- **Application**: CQRS handlers, DTOs, and business logic  
- **Infrastructure**: Database, Redis, Identity, external services  
- **API**: REST controllers and configuration  

CQRS is used to separate read and write operations, while the Specification Pattern handles flexible querying logic.

---

## Getting Started

### Backend
```bash
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend
```bash
npm install
ng serve
```

---

## Author

Ahmed Abdelmoaty  
GitHub: https://github.com/AhmedAbdelmoaty-dev  
LinkedIn: https://linkedin.com/in/ahmed-abdelmoaty-dev
