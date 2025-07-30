# Fundo - Loan Management System

A full-stack, modern, and secure Loan Management System built with **.NET 8 (Clean Architecture)** and **Angular 19**.

This project is designed as a take-home challenge for Full-Stack Developer roles, featuring robust authentication, rich validation, best engineering practices, and a clear separation of concerns.

---

## 🏗️ Tech Stack

| Layer      | Technology / Approach                      |
|------------|--------------------------------------------|
| Frontend   | Angular 19, TailwindCSS, Standalone Components |
| Backend    | ASP.NET Core 8 (.NET 8 / C# 13), MediatR   |
| ORM        | Entity Framework Core (SQL Server)         |
| Validation | FluentValidation                           |
| Auth       | JWT Bearer Token                           |
| Tests      | xUnit, Moq, FluentAssertions, Jasmine, Karma |
| DevOps     | Docker, Docker Compose, GitHub Actions     |
| Docs       | Swagger (with JWT Auth UI)                 |

---

## 🧩 Project Structure

```
repo-root/
│
├── backend/
│   └── src/
│       ├── Fundo.API           # Controllers, middleware, Swagger, Auth config
│       ├── Fundo.Application   # CQRS (commands/queries), DTOs, validation
│       ├── Fundo.Domain        # Core business logic and entities
│       ├── Fundo.Infrastructure# Persistence layer, EF Core
│       └── Fundo.Tests         # Unit & integration tests
│
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/     # Standalone Angular components (Loans, Auth, etc)
│   │   │   ├── services/       # AuthService, LoanService, interceptors
│   │   │   └── ...             # Main app setup
│   ├── tailwind.config.js
│   ├── angular.json
│   └── package.json
│
├── docker-compose.yml
└── README.md                   # (You're here!)
```

---

## 🚀 Running the Project (Dev Setup)

**Requirements:**
- [Docker](https://www.docker.com/)
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js (v20+)](https://nodejs.org/) + [Yarn](https://yarnpkg.com/) or npm

### 1. Clone the repository

```bash
git clone https://github.com/your-user/fundo-fullstack.git
cd fundo-fullstack
```

### 2. Start everything (API + SQL Server + Frontend)

```bash
docker-compose up --build
```
- API (Swagger): http://localhost:8080/swagger
- Angular UI: http://localhost:4200/

---

## 🗝️ Authentication Flow

All API endpoints are JWT-protected.

### Obtain a JWT

```http
POST http://localhost:8080/api/Auth/token
Content-Type: application/json

{
  "clientId": "fundo-app",
  "clientSecret": "dev-secret-123"
}
```

**Copy the returned token** and use it for all subsequent API calls:
```
Authorization: Bearer <your_token>
```

The Angular frontend performs this flow automatically via `AuthService`. The token is stored in localStorage and injected into all API requests.

---

## 🧪 Backend: Available Endpoints

| Method | Route                      | Description               |
|--------|----------------------------|---------------------------|
| POST   | `/api/Auth/token`          | Generate JWT token        |
| POST   | `/loans`                   | Create new loan           |
| GET    | `/loans`                   | Get all loans             |
| GET    | `/loans/{id}`            | Get loan by ID            |
| POST   | `/loans/{id}/payment`    | Register a payment        |

---

## ▶️ Running Locally: Backend (API)

From `backend/src`:

```bash
dotnet build
dotnet run --project Fundo.API
```

API docs at http://localhost:8080/swagger

To run all backend tests:

```bash
dotnet test
```

---

## ▶️ Running Locally: Frontend (Angular)

From `frontend`:

```bash
yarn install  # or npm install
yarn start    # or npm start
```

- App: http://localhost:4200/

### Frontend Features

- **Login is automatic**: the app requests a JWT with the demo credentials (`fundo-app` / `dev-secret-123`) on startup.
- **Loan list**: Shows all loans, using Angular standalone components, TailwindCSS, loading skeleton, error states.
- **Loan details**: View full details by clicking a loan, fetches by ID.
- **Register payment**: Submit payment for a loan.
- **Global error handling**: HTTP errors are handled and surfaced in the UI.
- **JWT Interceptor**: All requests send `Authorization: Bearer ...` header.

#### Key Files / Concepts

- `auth.service.ts` — handles login/token storage
- `loan.service.ts` — CRUD for loans, includes payment
- `auth.interceptor.ts` — attaches JWT to all outgoing requests
- `loans-list.component.ts` — shows the list (uses Angular material/table)
- `loan-detail.component.ts` — details for a single loan
- Uses TailwindCSS utility classes for styling and layout

---

## 🧪 Tests

- **Backend**: xUnit, Moq, FluentAssertions, in-memory integration tests
- **Frontend**: (TODO)

Run backend tests:

```bash
dotnet test
```

Run frontend tests:

```bash
yarn test
# or
npm run test
```

---

## 📦 Environment Variables

| Variable                       | Purpose                              |
|--------------------------------|--------------------------------------|
| `JWT_Secret`                   | Secret key for JWT                   |
| `ConnectionStrings__DefaultConnection` | SQL Server connection string     |

---

## 🐳 Docker & Docker Compose

All services can be brought up with:

```bash
docker-compose up --build
```

This will start:
- **SQL Server** (containerized)
- **.NET Backend** (API on :8080)
- **Angular Frontend** (on :4200)

---

## Swagger Demo Credentials

- **User**: `fundo-app`
- **Secret**: `dev-secret-123`

---

## 👩‍💻 Highlights

- Clean, domain-driven architecture
- Full JWT Auth with protected endpoints
- Rich validation (FluentValidation, error responses)
- Solid test coverage (unit + integration)
- Clean UI: Angular standalone components, Tailwind, JWT login, error handling
- Dockerized: fast local setup

