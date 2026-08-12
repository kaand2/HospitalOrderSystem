# Hospital Order System (HOS)

A RESTful API built with ASP.NET Core 8 for managing hospital patients, medical orders, and user authentication. The project implements Clean Architecture principles.

---

## Features

- **Patient Management:** CRUD operations for patient records.
- **Order Management:** Lifecycle management for medical orders (Laboratory, Radiology, Nursing, Medication, Diet).
- **State Machine Logic:** Validation rules for order status transitions.
- **Order Action Tracking:** Historical tracking of status changes and updates made to an order.
- **Authentication & Authorization:** JWT-based authentication with role-based access control (Admin, Doctor, Nurse, Laboratory, Radiology).
- **Row-Level Security:** Data access filtering based on department and role (e.g., Laboratory role accesses only Laboratory orders).
- **Global Error Handling:** Custom exception middleware that maps exceptions to standard HTTP status codes.

---

## Architecture & Technologies

The application is structured into five layers based on Clean Architecture:

1. **Domain:** Entities, enumerations, and exceptions.
2. **Application:** Business logic, DTOs, Validation, and Service interfaces.
3. **Infrastructure:** External services, JWT token generation, and password hashing.
4. **Persistence:** EF Core DbContext, Migrations, and Repositories.
5. **API:** Controllers, Middleware, and Dependency Injection configurations.

### Tech Stack
- **Framework:** .NET 8, ASP.NET Core Web API
- **Database:** Microsoft SQL Server, Entity Framework Core
- **Libraries:** AutoMapper, FluentValidation, BCrypt
- **Testing:** xUnit, Moq
- **Documentation:** Swagger / OpenAPI

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or a dedicated instance)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/HospitalOrderSystem.git
   cd HospitalOrderSystem
   ```

2. **Configure the Database**
   Update the `DefaultConnection` string in `appsettings.json` (inside the `HospitalOrderSystem.API` project) to point to your active SQL Server instance.

3. **Apply EF Core Migrations**
   ```bash
   dotnet ef database update --project HospitalOrderSystem.Persistence --startup-project HospitalOrderSystem.API
   ```

4. **Run the Application**
   ```bash
   dotnet run --project HospitalOrderSystem.API
   ```

5. **Explore the API**
   Navigate to `https://localhost:<port>/swagger` in your browser to view the interactive API documentation.

---

## Testing

The solution includes a testing project for controller logic, validation, and exception handling.

To run the tests:
```bash
dotnet test
```

---

## Authentication

The API requires JWT for protected endpoints. 
- Log in via the `AuthController` to receive a Bearer token.
- Provide the token in the format: `Bearer <your_token>` via the Authorization header or the Swagger UI.
