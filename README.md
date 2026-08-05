# dotnet-projects

A collection of small C#/.NET projects built while working through a .NET backend development course. Each folder is an independent project exploring a different concept — from language basics to a full Web API with JWT auth.

## Projects

| Project | Description |
|---|---|
| [HelloWorld](HelloWorld/README.md) | C# language fundamentals — data types, operators, arrays, collections, loops. |
| [FileApp](FileApp/README.md) | Reading/writing JSON and text files, and mapping between models with AutoMapper. |
| [TaskApp](TaskApp/README.md) | `Task` / `async`/`await` and basic multithreading examples. |
| [ModelsApp](ModelsApp/README.md) | Seeding SQL Server from JSON files using Dapper and EF Core side by side. |
| [SQLSeed](SQLSeed/README.md) | Standalone console app that seeds the course database from JSON data. |
| [DotnetAPI](DotnetAPI/README.md) | ASP.NET Core Web API with JWT authentication, Dapper/EF Core data access, and CRUD endpoints for Users and Posts. |

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download) (net9.0 for SQLSeed, net10.0 for the rest)
- SQL Server (local instance) for any project that talks to `DotNetCourseDatabase`

## Running a project

```bash
cd <ProjectName>
dotnet run
```

Each project directory contains its own README with project-specific setup notes.
