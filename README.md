# Hospital Order System (HOS)

A comprehensive Full-Stack application for managing hospital patients, medical orders, and user authentication. The project consists of a RESTful API built with ASP.NET Core 8 (Clean Architecture) and a modern frontend built with React and TypeScript.

---

## Features

### Backend
- **Patient Management:** CRUD operations for patient records with soft delete and validation rules.
- **Order Management:** Lifecycle management for medical orders (Laboratory, Radiology, Nursing, Medication, Diet).
- **State Machine Logic:** Validation rules for order status transitions (e.g., Draft -> Active -> In Progress -> Completed/Cancelled).
- **Order Action Tracking:** Historical tracking of status changes and updates made to an order.
- **Authentication & Authorization:** JWT-based authentication with role-based access control (Admin, Doctor, Nurse, Laboratory, Radiology).
- **Global Error Handling:** Custom exception middleware that maps exceptions to standard HTTP status codes.

### Frontend
- **Modern User Interface:** Built with React, Tailwind CSS, and shadcn/ui for a clean, responsive, and accessible design.
- **Robust State Management:** Uses TanStack React Query for efficient data fetching, caching, and mutation state handling.
- **Form Handling & Validation:** Integrated with React Hook Form for seamless user input and validation.
- **Localization (i18n):** Multi-language support implemented using `react-i18next`.
- **Type-Safe API Integration:** Strictly typed models and fetch clients that perfectly map to the backend DTOs.

---

## Architecture & Technologies

### Backend Tech Stack
The backend is structured into five layers based on Clean Architecture:
1. **Domain:** Entities, enumerations, and exceptions.
2. **Application:** Business logic, DTOs, Validation, and Service interfaces.
3. **Infrastructure:** External services, JWT token generation, and password hashing.
4. **Persistence:** EF Core DbContext, Migrations, and Repositories.
5. **API:** Controllers, Middleware, and Dependency Injection configurations.

- **Framework:** .NET 8, ASP.NET Core Web API
- **Database:** Microsoft SQL Server, Entity Framework Core
- **Libraries:** AutoMapper, FluentValidation, BCrypt
- **Testing:** xUnit, Moq
- **Documentation:** Swagger / OpenAPI

### Frontend Tech Stack
- **Framework:** React 18, Vite
- **Language:** TypeScript
- **Styling:** Tailwind CSS, shadcn/ui, Lucide Icons
- **State Management:** TanStack React Query
- **Routing:** React Router DOM
- **Localization:** i18next

---

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or a dedicated instance)
- [Node.js](https://nodejs.org/) (for the frontend)

### 1. Backend Setup

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

4. **Run the API**
   ```bash
   dotnet run --project HospitalOrderSystem.API
   ```
   Navigate to `https://localhost:<port>/swagger` to view the interactive API documentation.

### 2. Frontend Setup

1. **Navigate to the frontend directory**
   ```bash
   cd ../hospital-order-frontend
   ```

2. **Install Dependencies**
   ```bash
   npm install
   ```

3. **Configure Environment Variables**
   Ensure your `.env` file points to the running backend API URL (e.g., `VITE_API_BASE_URL=https://localhost:<port>`).

4. **Run the Development Server**
   ```bash
   npm run dev
   ```
   Open the provided local URL (typically `http://localhost:5173`) in your browser.

---

## Authentication Workflow

The system requires JWT for protected endpoints. 
- You can create an initial admin user via the `bootstrap-admin` endpoint if the database is empty.
- Log in via the AuthController (or the frontend login page) to receive a Bearer token.
- The frontend will automatically attach this token to subsequent requests using the custom `fetch-client`.

## Testing (Backend)

The solution includes a testing project for controller logic, validation, and exception handling.

To run the backend tests:
```bash
dotnet test
```
