# ModelsApp

A C# console app that seeds a SQL Server database (`TutorialAppSchema`) from JSON files, using both **Dapper** and **Entity Framework Core** data access (`Data/DataContextDapper.cs`, `Data/DataContextEF.cs`) so the two approaches can be compared side by side.

On run, it:

1. Executes `Users.sql` to (re)create the `Users` table.
2. Deserializes `Users.json`, `UserSalary.json`, and `UserJobInfo.json`.
3. Batches and inserts each set of rows into SQL Server via Dapper, chunking statements to stay under SQL Server's batch size limits.

Model classes for `Users`, `UserSalary`, `UserJobInfo`, and `Computer` live in `Models/`.

## Requirements

- .NET SDK (net10.0)
- SQL Server (connection string in `appsettings.json`, database: `DotNetCourseDatabase`)

## Running

```bash
dotnet run
```

Prints `SQL Seed Completed Successfully` once all tables have been seeded.
