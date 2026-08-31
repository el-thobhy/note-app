# 📝 Daily Note & Admin Management Dashboard (.NET 8 MVC)

A modern, enterprise-ready web application built with **ASP.NET Core 8.0 MVC** that serves as an **Administrator Management System** and **Real-Time Communication Hub**. Designed to demonstrate clean architecture, robust security, and seamless backend API integration for .NET Developer evaluation.

---

## 📌 Executive Summary / Overview

This project was built to address two core requirements:
1. **Administrative User & Role Management**: An intuitive dashboard for managing user accounts, dynamic role permissions, and secure account lifecycle management.
2. **Real-time Live Chat & Notifications**: Bidirectional messaging using **ASP.NET Core SignalR** with JWT-authenticated WebSocket connections and message persistence.

---

## 🚀 Key Features

### 1. 🔐 Authentication & Session Security
- **JWT & Session Hybrid Flow**: Decodes and stores JWT token claims securely in HTTP-only, server-side distributed memory sessions (`IDistributedCache`).
- **CSRF / XSRF Protection**: Implemented `[ValidateAntiForgeryToken]` on critical state-modifying requests (`POST`, `DELETE`).
- **Role-Based Access Control (RBAC)**: Dynamic role extraction from JWT claims (`JwtHelper`) to restrict admin-only endpoints and views.
- **OTP Verification & Account Registration**: Endpoints for email-based OTP verification and account provisioning.

### 2. 👥 User & Role Management Dashboard
- **Interactive DataTables Integration**: Asynchronous server-side/client-side data rendering with search, sort, and pagination.
- **Role Elevation / Modification**: Modal-driven updates for user roles with instant UI feedback and error handling.
- **Safe Account Deletion**: Double-confirmation deletion flow with modal alerts and AJAX dispatch.

### 3. 💬 Real-Time Messaging & Presence (SignalR)
- **Hub WebSocket Connection**: Auto-reconnecting SignalR connection passing JWT bearer tokens via `accessTokenFactory`.
- **Online Presence Detection**: Real-time broadcasts when users connect or disconnect (`UserConnected`, `UserDisconnected`).
- **1-on-1 Private Messaging**: Instant message delivery with conversation history retrieval from upstream microservices.

### 4. 🐳 DevOps & Containerization
- **Multi-stage Dockerfile**: Optimized .NET 8 runtime & SDK multi-stage build for minimal container footprint and quick deployment.

---

## 🛠️ Tech Stack & Architecture

| Layer | Technology |
|---|---|
| **Framework** | .NET 8.0 (C# 12) |
| **App Model** | ASP.NET Core MVC |
| **Real-time Communication** | SignalR (WebSockets / Long Polling fallback) |
| **State & Session** | In-Memory Distributed Cache (`IDistributedCache`), Server-side Cookies |
| **HTTP Client Architecture** | `IHttpClientFactory` + Typed / Scoped Service Layer (`IAccountService`, `IChatService`) |
| **Serialization** | `Newtonsoft.Json` / `System.Text.Json` |
| **Frontend & UI** | Razor Views (`.cshtml`), Bootstrap 5, jQuery DataTables, UI Avatars API |
| **DevOps / Hosting** | Docker (Linux containers), Azure Container Tools |

---

## 📂 Project Architecture

```plaintext
├── Controllers/              # MVC Controllers
│   ├── AuthController.cs     # Login, OTP verification, Logout & Session management
│   ├── HomeController.cs     # Dashboard & Admin User/Role management actions
│   └── ChatController.cs     # Live chat routing & Chat history bridge
├── Services/                 # Business logic & External API Clients (Separation of Concerns)
│   ├── LoginService.cs       # Auth API communication & OTP handlers
│   ├── HomeServices.cs       # Account and Role API handlers (IAccountService)
│   └── ChatServices.cs       # Chat history & Messaging API handlers (IChatService)
├── ViewModel/                # Strongly-typed Data Transfer & Request/Response Models
├── Helper/                   # Utility classes (e.g., JwtHelper for claim parsing)
├── Views/                    # Razor View templates & Layouts
│   ├── Auth/                 # Login & Registration screens
│   ├── Home/                 # Account management DataTables view
│   └── Chat/                 # SignalR Chat room interface
├── wwwroot/                  # Static assets (CSS, JS, Vendor libraries)
├── Dockerfile                # Multi-stage Docker build config
└── Program.cs                # Dependency Injection (DI) container & middleware pipeline
```

---

## ⚙️ Design Decisions & Best Practices Highlighted

1. **Dependency Injection & Interface Segregation**:
   - `IAccountService` and `IChatService` are registered with scoped lifetimes in `Program.cs`.
   - Controller logic stays lean and testable by delegating external REST calls to dedicated service classes.

2. **Resilient HTTP Communication**:
   - Uses `IHttpClientFactory` to prevent socket exhaustion issues under high concurrent loads.
   - Centralized Bearer Token injection from active session headers into outgoing microservice requests.

3. **Security First**:
   - Session cookies configured with `HttpOnly = true` and `IsEssential = true` to mitigate XSS-based session hijacking.
   - Antiforgery tokens passed through AJAX headers (`RequestVerificationToken`) to prevent CSRF attacks.

---

## 🚦 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/) (Optional, for containerized run)
- Running Backend REST API / SignalR Hub (Target URL configured in `appsettings.json`)

### 1. Configuration
Set your backend API endpoint in `appsettings.json` or `appsettings.Development.json`:
```json
{
  "ApiUrl": "https://your-backend-api.com",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 2. Run Locally via CLI
```bash
# Clone the repository
git clone https://github.com/your-username/note-app.git

# Navigate to project root
cd note-app

# Restore NuGet dependencies
dotnet restore

# Run the application
dotnet run
```
Access the application at `http://localhost:5000` or `https://localhost:5001`.

### 3. Run with Docker
```bash
# Build the Docker image
docker build -t admin-note-app .

# Run the container
docker run -d -p 8080:8080 --name admin-note-app-container admin-note-app
```
Access via `http://localhost:8080`.
