# Contributing

This solution is the standard template for .NET projects, so every change must preserve its predictable structure. See [README.md](README.md) for the architecture overview, [TESTING.md](TESTING.md) for the required TDD workflow, and [REVIEW.md](REVIEW.md) for what reviewers check.

## Architecture Rules

- **Domain has no dependencies.** It must not reference DataAccess, Api, or external infrastructure packages.
- **Api must not reference DataAccess.** Controllers depend only on Domain interfaces.
- **IoC is the only entry point.** `Program.cs` lives in `DotNetExample.IoC`, never in the Api project.
- **Centralized DI.** All registrations are composed in `DotNetExample.IoC/RegisterDependencies.cs`. Layers may expose `services.Add[LayerName]()` extension methods (e.g. `ApiConfiguration.AddApi()`), which IoC calls.
- **Repository + Wrapper pattern.** DataAccess repositories implement Domain repository interfaces and access the underlying store (HTTP, SQL, Mongo, clock, …) only through a wrapper interface such as `IHttpClientWrapper`. This lets tests use real repositories with mocked low-level drivers.

## Design Patterns

### CQRS

All business logic is split into Commands (writes) and Queries (reads):

- **Commands**: `DotNetExample.Domain/Commands`, return `Task`.
- **Queries**: `DotNetExample.Domain/Queries`, return `Task<T>`.
- Name handlers `VerbNoun` (e.g. `GetFoo`, `SetFoo`), each implementing its own interface (`IGetFoo`).
- Inject repository interfaces through the constructor.

### Naming

- Interfaces are PascalCase prefixed with `I` (e.g. `IFooRepository`).
- Async methods end with `Async` (e.g. `GetFooAsync`).

## Build Configuration

Every project must set:

```xml
<ImplicitUsings>enable</ImplicitUsings>
<Nullable>enable</Nullable>
<TreatWarningsAsErrors>True</TreatWarningsAsErrors>
```

## Development Workflow

1. **Understand the requirement** before writing anything.
2. **Plan the first failing test.**
3. **Run the Red/Green/Refactor cycle** described in [TESTING.md](TESTING.md), repeating until the feature is complete. Writing implementation code without a verified failing test is not allowed.
4. **Work in small steps.** Don't generate large amounts of code at once, and don't anticipate future requirements — solve only the current failing test.
5. **Check conformity.** New files must land in the correct layer and folder and use the matching namespace.
6. **Register new types** in `RegisterDependencies.cs`.
