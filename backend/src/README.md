# Fundo API

A simple, clean and secure Loan Management API built with .NET 8 and Clean Architecture.  
It supports loan creation, payment registration, and JWT-protected endpoints — with solid validations and a robust CQRS structure.

---

- Clean and decoupled architecture
- Domain-centric modeling
- Secure API using JWT
- Strong validation and error handling
- Fully tested (unit and integration)
- Dockerized for easy setup

---

## 🔧 Stack & Patterns

| Layer           | Tech / Approach                         |
|----------------|------------------------------------------|
| Framework       | ASP.NET Core 8 (.NET 8 / C# 13)          |
| Architecture    | Clean Architecture + CQRS (MediatR)      |
| ORM             | Entity Framework Core (SQL Server)       |
| Validation      | FluentValidation                         |
| Auth            | JWT Bearer Token                         |
| Tests           | xUnit + Moq + FluentAssertions           |
| DevOps          | Docker, Docker Compose                   |
| Docs            | Swagger (with JWT Auth UI)               |

---

## 🧱 Project Structure

```
src/
├── Fundo.API              → Controllers, middlewares, Swagger, Auth config
├── Fundo.Application      → CQRS (commands/queries), DTOs, validation
├── Fundo.Domain           → Core business logic and entities
├── Fundo.Infrastructure   → EF Core, persistence layer, interfaces
├── Fundo.Tests            → Unit & Integration tests
```

---

## 🗝️ Authentication

All endpoints are protected with JWT.  
You must authenticate first using:

```http
POST /api/Auth/token
{
  "clientId": "fundo-app",
  "clientSecret": "dev-secret-123"
}
```

The token should be sent in subsequent requests via:

```
Authorization: Bearer <your_token>
```

---

## 🧪 Available Endpoints

| Method | Route                      | Description               |
|--------|----------------------------|---------------------------|
| POST   | `/api/Auth/token`          | Generate JWT token        |
| POST   | `/loans`                   | Create new loan           |
| GET    | `/loans`                   | Get all loans             |
| GET    | `/loans/{id}`              | Get loan by ID            |
| POST   | `/loans/{id}/payment`      | Register a payment        |

---

## ▶️ Running locally

Make sure you have **Docker** and **.NET 8 SDK** installed.

```bash
# clone the repo
git clone https://github.com/your-user/fundo-api.git
cd fundo-api

# run everything (API + SQL Server)
docker-compose up --build
```

API will be available at:  
http://localhost:8080/swagger

Swagger credentials (if applicable):  
**User**: `fundo-app`  
**Secret**: `dev-secret-123`

---

## 🧪 Running Tests

```bash
dotnet test
```

Covers:

- ✅ Application layer unit tests
- ✅ Integration tests via in-memory server
- ✅ Error cases and validation paths

---

## 📦 Environment

| Variable        | Purpose                         |
|----------------|----------------------------------|
| `JWT_Secret`   | Secret key for token generation  |
| `ConnectionStrings__DefaultConnection` | SQL Server connection string |

---

## Running the Backend

To build the backend, navigate to the `src` folder and run:
```sh
dotnet build
```

To run all tests:
```sh
dotnet test
```

To start the main API:
```sh
cd Fundo.API  
dotnet run
```

The following endpoint should return **200 OK**:
```http
GET -> https://localhost:8080/loans
```