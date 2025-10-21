# Fundo Loan Management Application

This is a simple loan management system built with **.NET 6** and **Angular**, designed to demonstrate a clean architecture approach and several modern backend patterns. The goal of this project was to implement core features with clean, maintainable code, leaving room for future improvements.

---

## Architecture Overview

Below is a simplified diagram of the project's **Clean Architecture**:
```
FundoLoanApp/
├── WebApi/                  # Controllers, Routing, HTTP
│   └── LoanManagementController.cs
│
├── Application/             # Use Cases, MediatR Handlers, Pipeline Behaviors
│   ├── Handlers/
│   │   ├── CreateLoanHandler.cs
│   │   ├── GetLoanHandler.cs
│   │   ├── CreatePaymentHandler.cs
│   │   └── GetAllLoansHandler.cs
│   └── Behaviors/
│       ├── ExceptionHandlingBehavior.cs
│       ├── LoggingBehavior.cs
│       └── ValidationBehavior.cs
│
├── Domain/                  # Entities, Enums, Result Pattern
│   ├── Entities/
│   │   ├── Loan.cs
│   │   └── LoanPayment.cs
│
└── Infrastructure/          # Repositories, Database
    ├── Repositories/
    │   └── LoanRepository.cs
    └── DbContext/
        └── FundoDbContext.cs
```

---

## Technical Choices

### Backend

- **Clean Architecture**: Separates **Application**, **Domain**, **Infrastructure**, and **WebApi** layers for maintainability and testability.
  
- **MediatR**: Commands and queries are handled via MediatR to decouple controllers from business logic.

- **Documentation Comments**: XML comments describe parameters, return types, and HTTP responses.

- **Pipeline Behaviors (MediatR)**: Centralized handling for:
  - Exception catching and consistent response conversion.
  - Logging of requests and responses.
  - Validation (via FluentValidation or custom validators) applied automatically per request.

- **Result Pattern**:  
  - Returns expected errors (e.g., “Loan not found”) as `Result<T>`.
  - Reserves exceptions for unexpected errors only.
  - [Why results instead of exceptions?](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)

- **Result Serializer**: Converts `Result` objects into consistent HTTP responses.

- **Database**:  
  - EF Core with **In-Memory Database** for tests.
  - Can be migrated to a real database easily.

- **Authentication**: Not implemented for simplicity. Adding `[Authorize]` in ASP.NET Core would be trivial.

---

### Frontend

- **Simplicity first**: Built according to requirements.
- **Pagination support**: Added to loan list as per backend contract.
- **Potential improvements**: A detailed loan view (with payment history and dates) could be added since backend provides this data.

---

## Testing

- **Unit tests**: Covers core business logic.
- **Integration tests**: Uses EF Core In-Memory DB and mocked MediatR. Could be improved with more scenarios if more time were available.

---

## Future Improvements

- Add authentication and authorization flows.
- Enhance frontend with detailed loan and payment views.
- Improve integration test coverage.
- Use real databases and migrations for production.

---

## How to Run

1. **Backend**:  
   - Requires docker and docker-compose.  
   - Run:  
     ```bash
     dotnet-compose up
     ```

---

This project focuses on **clean architecture, maintainable patterns, and predictable business logic**, providing a strong foundation for future expansion.
