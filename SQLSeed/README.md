# SQLSeed

A standalone console app that seeds the `DotNetCourseDatabase` SQL Server database from JSON fixture files, using Dapper only (no EF Core). Functionally the same seeding logic as `ModelsApp`, extracted into its own project.

On run, it:

1. Executes `Users.sql` to (re)create the `Users` table.
2. Deserializes `Users.json`, `UserSalary.json`, and `UserJobInfo.json`.
3. Batches and inserts each set of rows into SQL Server, chunking statements to stay under SQL Server's batch size limits.

## Requirements

- .NET SDK (net9.0)
- SQL Server (connection string in `appsettings.json`)

## Running

```bash
dotnet run
```

Prints `SQL Seed Completed Successfully` once all tables have been seeded.
