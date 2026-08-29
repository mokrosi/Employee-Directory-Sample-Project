# Employee Directory Application


## Tech Stack

- **Backend**:
  - **.NET 10 Web API**
  - **Clean Architecture** (Domain, Application, Infrastructure, API layers)
  - **CQRS Pattern** using MediatR
  - **FluentValidation** for input validation and business rules
  - **Entity Framework Core** with SQL Server (LocalDB)
  - **JWT Authentication** & BCrypt password hashing
  - **xUnit & Moq** for unit testing

- **Frontend**:
  - **Angular (Standalone Components)**
  - **RxJS** for reactive state management
  - **Reactive Forms** with client-side validation
  - **Angular Router** with Auth Guards and Route Resolvers
  - **TypeScript** & Vanilla CSS

---

## Prerequisites

Ensure you have the following installed on your machine:
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18 or higher) & npm
- **SQL Server LocalDB** (included with Visual Studio)

---

## How to Run the Project

### 1. Start the Backend API

Open a terminal and run:

```powershell
cd backend\EmployeeDirectory
dotnet run
```

The API will start and listen on:
- **HTTPS:** `https://localhost:7214`
- **HTTP:** `http://localhost:5290`

*(Database tables and seed data will be automatically created on startup).*

---

### 2. Start the Frontend Application

Open a second terminal and run:

```powershell
cd frontend
npm install
npm start
```

The Angular development server will start at:
- **`http://localhost:4200`**

---

## Getting Started / User Flow

1. Open your browser and go to **`http://localhost:4200`**.
2. **Register**: Go to `/register` and create an account:
   - Provide your **Full Name**, **Email**, and **Password** (min 8 chars, 1 uppercase, 1 digit).
3. **Login**: Sign in at `/login` with your registered credentials.
4. **Features Available**:
   - **Employees (`/employees`)**: Search employees in real-time, click **+ Add New Employee** to create employees via modal, click **View** to see profiles, and click **Transfer** to reassign departments with live capacity feedback.
   - **Departments (`/departments`)**: View capacity utilization, add new departments, and update capacity limits.
   - **History (`/history`)**: View the complete audit log of all employee assignments and transfers with performer names, emails, and timestamps.

---

## Running Backend Tests

To run the unit test suite:

```powershell
cd backend
dotnet test
```
