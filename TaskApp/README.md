# TaskApp

A minimal C# console app demonstrating `Task`, `async`/`await`, and basic multithreading:

- Starting a `Task` explicitly with `.Start()` vs. using an `async` method.
- Awaiting multiple tasks and observing interleaved completion order based on delay.
- `Thread.Sleep` vs. `Task.Delay`.

## Requirements

- .NET SDK (net10.0)

## Running

```bash
dotnet run
```

Expected output shows "Task 3", "Task 2", "Task 1" (or similar) completing out of declaration order based on their delay times.
