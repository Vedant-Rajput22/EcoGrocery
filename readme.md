# 🌱 EcoGrocery – Backend

&#x20;&#x20;

> **An eco‑friendly grocery platform** focused on sustainability metrics, rich filtering, and seamless Stripe Checkout – built with **ASP.NET Core** (+ MediatR, EF Core, Identity) and **MS SQL Server**.

---

## ✨ Key Features

| Area                   | Highlights                                                           |
| ---------------------- | -------------------------------------------------------------------- |
| **Authentication**     | JWT‑based signup & login with ASP.NET Identity                       |
| **Products**           | Carbon‑footprint per item, eco‑labels (Plastic‑Free, Organic, Local) |
| **Cart & Orders**      | Add / update / remove items, place order, order history              |
| **Payments**           | Stripe Payment Intent flow, webhook → mark order **Paid**            |
| **Admin**              | CRUD products, adjust stock, manage eco‑labels                       |
| **Clean Architecture** | `Domain` → `Application` → `Infrastructure` → `Api` layers           |
| **Cloud‑ready**        | Stateless API, env‑driven config, Docker‑friendly                    |

---

## 🛠 Tech Stack

```text
Backend   : ASP.NET Core 8, MediatR, FluentValidation, AutoMapper
Database  : MS SQL Server + EF Core
Payments  : Stripe .NET v48
Auth      : ASP.NET Identity + JWT
Tooling   : dotnet‑ef, git‑filter‑repo, Stripe CLI
CI/CD     : GitHub Actions (sample workflow in /.github)
```

---

## 🚀 Quick Start (Local)

1. **Clone & restore packages**

   ```bash
   git clone https://github.com/Vedant-Rajput22/EcoGrocery.git
   cd EcoGrocery
   dotnet restore
   ```
2. **Configure secrets** (never commit secrets!)

   ```bash
   cd Api                # project with Program.cs
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:Default" "Server=.;Database=EcoDb;Trusted_Connection=True;TrustServerCertificate=True"
   dotnet user-secrets set "Stripe:SecretKey"  "sk_test_xxx"
   dotnet user-secrets set "Stripe:WebhookKey" "whsec_xxx"
   dotnet user-secrets set "Jwt:Key" "super‑secret‑jwt‑key"
   ```
3. **Run migrations & seed**

   ```bash
   dotnet ef database update --project Infrastructure
   ```
4. **Run the API**

   ```bash
   dotnet run --project Api
   # Swagger UI → https://localhost:7192/swagger
   ```
5. **Test payments**

   ```bash
   stripe listen --forward-to https://localhost:7192/api/Payments/webhook --skip-verify
   # In another terminal
 stripe payment_intents confirm <pi_id> --payment-method pm_card_visa --return-url https://example.com/success
  ```

6. **Run the frontend UI**

   ```bash
   cd frontend
   npm install
   npm run dev
   ```

---

## 🔑 Environment Variables

| Key                          | Purpose                       | Example                                           |
| ---------------------------- | ----------------------------- | ------------------------------------------------- |
| `ConnectionStrings__Default` | SQL Server connection         | `Server=.;Database=EcoDb;Trusted_Connection=True` |
| `Stripe__SecretKey`          | Stripe secret API key         | `sk_test_…`                                       |
| `Stripe__WebhookKey`         | Stripe webhook signing secret | `whsec_…`                                         |
| `Jwt__Key`                   | JWT signing key (≥128 bits)   | `p6lhT9…`                                         |
| `Jwt__Issuer` / `Audience`   | Token issuer / audience       | `EcoGroceryApi` / `EcoGroceryClient`              |

Use **User Secrets** for local dev and **env vars** (or secrets store) in production.

---

## 🌐 REST API Overview

| Verb   | Endpoint                       | Description                  | Auth     |
| ------ | ------------------------------ | ---------------------------- | -------- |
| POST   | /api/Auth/register             | Register new customer        | —        |
| POST   | /api/Auth/login                | Obtain JWT                   | —        |
| GET    | /api/Products                  | List / filter products       | Optional |
| GET    | /api/Cart                      | Get active cart              | Customer |
| POST   | /api/Cart/items                | Add item to cart             | Customer |
| PUT    | /api/Cart/items/{productId}    | Update item quantity         | Customer |
| DELETE | /api/Cart/items/{productId}    | Remove item from cart        | Customer |
| GET    | /api/Orders                    | List orders for current user | Customer |
| GET    | /api/Orders/all                | List **all** orders          | Admin    |
| POST   | /api/Orders/checkout/{cartId}  | Place order from cart        | Customer |
| PATCH  | /api/Orders/{id}/status        | Update order status          | Admin    |
| POST   | /api/Payments/intent/{orderId} | Create Stripe PaymentIntent  | Customer |
| POST   | /api/Payments/webhook          | Stripe webhook endpoint      | Stripe   |
| POST   | /api/Products                  | Create product               | Admin    |

Full Swagger docs are available at **/swagger** when the API runs.

## 🧪 Testing Cheatsheet Testing Cheatsheet

```bash
# Run unit tests (WIP)
dotnet test

# Re‑generate database
dotnet ef database drop -f
```

---

## 🤝 Contributing

1. Fork → feature branch → PR.
2. Format code with `dotnet format`.
3. Commits follow **Conventional Commits**.
4. No secrets in commits – GitHub Push Protection is enforced.

See **CONTRIBUTING.md** for full guidelines.

---

## 📝 License

This project is licensed under the **MIT License** – see `LICENSE` for details.

---

## 🙏 Acknowledgements

* [Stripe](https://stripe.com/) for the generous test environment.
* Inspired by the community’s push towards sustainable commerce.

> Made with ♥ by **Vedant Dipakkumar Rajput**
