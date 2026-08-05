# FileApp

A C# console app for practicing file I/O and object mapping:

- Reading/writing JSON files with both `System.Text.Json` and `Newtonsoft.Json`.
- Reading/writing plain text files (`log.txt`, `computersCopy*.txt`).
- Mapping between a snake_case JSON model (`ComputerSnake`) and a PascalCase C# model (`Computer`) using AutoMapper.
- Building SQL insert statements from a model and executing them against SQL Server via Dapper (`Data/DataContextDapper.cs`).

## Requirements

- .NET SDK (net10.0)
- SQL Server, if using the Dapper/database-backed sections (connection string in `appsettings.json`)

## Running

```bash
dotnet run
```

`Program.cs` reads `ComputersSnake.json`, maps each entry to a `Computer`, and prints the motherboard for each one. Other exercises (writing SQL insert statements to `log.txt`) are commented out — uncomment to try them.
