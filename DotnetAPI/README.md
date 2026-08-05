# DotnetAPI

An ASP.NET Core Web API (net10.0) implementing a small Users/Posts backend with JWT authentication, backed by SQL Server via both Dapper and Entity Framework Core.

## Features

- **JWT authentication** — register, login, and refresh-token endpoints (`AuthController`), with password hashing in `Helpers/AuthHelper.cs`.
- **Users CRUD** — implemented three ways for comparison:
  - `UserController` — Dapper, raw SQL.
  - `UserEFController` — Entity Framework Core.
  - `UserCompleteController` — Dapper via stored procedures, using the `IUserRepository`/`UserRepository` pattern.
- **Posts CRUD** — `PostController` (raw SQL) and `PostSPController` (stored procedures), including search and per-user queries.
- **Swagger/OpenAPI** UI enabled in development.
- CORS policies for local dev (`localhost:4200/3000/8000`) and a placeholder production origin.

## Requirements

- .NET SDK (net10.0)
- SQL Server (connection string + JWT signing keys in `appsettings.json`, database: `DotNetCourseDatabase`)

## Running

```bash
dotnet run
```

In development, Swagger UI is available at the app's root/`/swagger` for exploring and testing endpoints. `DotnetAPI.http` also contains sample requests you can run directly from an editor with REST client support.

## Project layout

- `Controllers/` — API endpoints
- `Data/` — `DataContextDapper`, `DataContextEF`, and the `IUserRepository`/`UserRepository` abstraction
- `Dtos/` — request/response DTOs for auth, users, and posts
- `Models/` — `User`, `UserComplete`, `Post`, `UserJobInfo`, `UserSalary`
- `Helpers/AuthHelper.cs` — password hashing/verification
