# Fundo Frontend (Angular) + Fundo API

Sample interface for consuming the **Fundo API** (Loan Management) using Angular 19, Angular Material, and TailwindCSS.

---

## ✨ Main Features

- Loan listing
- Loan details by ID
- Registering payments
- Complete JWT authentication integration (login via clientId / clientSecret)
- Modern UI (Angular Material + TailwindCSS)
- Clean, extensible structure (standalone components, best practices)

---

## 🛠️ Stack

| Layer     | Tech / Approach                 |
|-----------|---------------------------------|
| Frontend  | Angular 19 + Angular Material   |
| CSS       | TailwindCSS                     |
| API Auth  | JWT Bearer Token                |
| Calls     | HttpClient, Interceptor, RxJS   |

---

## 🚀 Running the Frontend

```sh
# Install dependencies
npm install

# Start the local server
npm start
# or
ng serve
```

Visit [http://localhost:4200](http://localhost:4200)

---

## ⚡ Project Structure

```
src/
  app/
    services/
      auth.service.ts         # Login, JWT token management
      loan.service.ts         # Calls Fundo API with token
      auth.interceptor.ts     # Injects JWT into requests
    components/
      loan-list/              # Loan listing
      loan-details/           # Loan details by ID
      payment-form/           # Register a payment
    pages/
      home/
      login/
    app.module.ts
    app-routing.module.ts
  styles.scss                 # TailwindCSS custom
  environments/               # API config
```

---

## 🎨 TailwindCSS + Angular Material

- **Tailwind**: already set up, edit your `styles.scss` as needed.
- **Angular Material**: available for tables, buttons, etc.

---

## 🔒 **Authentication Flow (Login/JWT)**

1. **Login** (get your JWT):

```typescript
// auth.service.ts (example)
login(clientId: string, clientSecret: string): Observable<void> {
  return this.http.post<{ token: string }>(`${API_URL}/api/Auth/token`, { clientId, clientSecret })
    .pipe(
      tap(res => localStorage.setItem('jwt', res.token))
    );
}
```

2. **Inject JWT into requests**:

```typescript
// auth.interceptor.ts
intercept(req, next) {
  const token = localStorage.getItem('jwt');
  if (token) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }
  return next.handle(req);
}
```

3. **Consume authenticated API**:

```typescript
// loan.service.ts
getLoans(): Observable<Loan[]> {
  return this.http.get<Loan[]>(`${API_URL}/loans`);
}
```

---

## 🔗 **Available Endpoints**

| Method | Route                      | Description         |
|--------|----------------------------|---------------------|
| POST   | `/api/Auth/token`          | Generate JWT token  |
| GET    | `/loans`                   | List all loans      |
| GET    | `/loans/{id}`              | Get loan by ID      |
| POST   | `/loans/{id}/payment`      | Register payment    |

---

## 🧑‍💻 **Usage Example (API consumption in Angular)**

```typescript
// Example: Fetching loans (loan.service.ts)
@Injectable({ providedIn: 'root' })
export class LoanService {
  constructor(private http: HttpClient) {}

  getLoans(): Observable<Loan[]> {
    return this.http.get<Loan[]>(`${environment.apiUrl}/loans`);
  }
}
```

```typescript
// Example: Login (auth.service.ts)
login(clientId: string, clientSecret: string): Observable<void> {
  return this.http.post<{ token: string }>(
    `${environment.apiUrl}/api/Auth/token`,
    { clientId, clientSecret }
  ).pipe(
    tap(res => localStorage.setItem('jwt', res.token))
  );
}
```

---

## 📦 **Environment Variables**

Edit `src/environments/environment.ts`:

```ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:8080'
};
```

---

## 🐳 **Run Everything with Docker**

1. **Start backend + DB:**
   ```sh
   docker-compose up --build
   ```
2. **Start frontend (in another terminal):**
   ```sh
   npm start
   ```

---

## 🛡️ **Integration Tips**

- Always login to obtain a token before accessing protected endpoints.
- The JWT token must be sent in the `Authorization` header of all authenticated requests.
- The frontend handles 401 errors (redirects to login when unauthorized).

---

## 🧪 **Frontend Testing**

```sh
npm test
# or
ng test
```
- Component, service, and HttpClient integration tests.
