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

- **Commands**: `DotNetExample.Domain/Commands/<Feature>`, return `Task`.
- **Queries**: `DotNetExample.Domain/Queries/<Feature>`, return `Task<T>`.
- Name handlers `VerbNoun` (e.g. `GetCar`, `AddCar`), each implementing its own interface (`IGetCar`).
- Inject repository interfaces through the constructor.

### Feature Folders

Inside each layer, group code into a folder per feature, and make the namespace match the folder path. A reader looking for everything about cars finds it under a `Cars/` folder in every layer.

| Kind | Path | Namespace |
|---|---|---|
| Command | `DotNetExample.Domain/Commands/Cars/AddCar.cs` | `DotNetExample.Domain.Commands.Cars` |
| Query | `DotNetExample.Domain/Queries/Cars/GetCar.cs` | `DotNetExample.Domain.Queries.Cars` |
| Repository interface | `DotNetExample.Domain/DataAccess/Cars/ICarRepository.cs` | `DotNetExample.Domain.DataAccess.Cars` |
| Entity / value object | `DotNetExample.Domain/Entities/Cars/Car.cs` | `DotNetExample.Domain.Entities.Cars` |
| Domain exception | `DotNetExample.Domain/Exceptions/Cars/CarNotFoundException.cs` | `DotNetExample.Domain.Exceptions.Cars` |
| Repository | `DotNetExample.DataAccess/Cars/CarRepository.cs` | `DotNetExample.DataAccess.Cars` |
| Request/response DTO | `DotNetExample.Api/Models/Cars/CarRequest.cs` | `DotNetExample.Api.Models.Cars` |

Two exceptions stay flat:

- **Controllers** (`Api/Controllers/CarsController.cs`): a controller already *is* one feature.
- **Shared infrastructure** that no single feature owns, such as `DataAccess/Wrappers/`.

### Naming

- Interfaces are PascalCase prefixed with `I` (e.g. `IFooRepository`).
- Async methods end with `Async` (e.g. `GetFooAsync`).

## Clean Code

Code is read far more often than it is written, so optimize for the reader.

### Readability

- **Code should read like a sentence.** A caller should be able to follow the flow from method and variable names alone, e.g. `if (order.IsOverdue()) await NotifyCustomerAsync(order);`.
- **Keep functions small.** Each function does one thing at one level of abstraction. When a function needs a section break, or you're tempted to explain a block, extract it into a well-named method.
- **Choose descriptive names over short ones.** Names say what something does or represents (`CalculateShippingCost`, `activeCustomers`), not how it's built (`DoWork`, `list2`, `tmp`). Avoid abbreviations and generic names like `Helper`, `Manager`, or `Data`.

### Comments

Comments should almost never be needed. If code needs a comment to explain *what* it does, rename or extract until it doesn't.

- **Allowed: comments that explain *why*.** Use them for code that looks wrong or surprising but has to be that way, e.g. unusual startup configuration that works around a parsing issue, or an ordering that a library requires. Link to the issue or docs where possible.
- **Allowed: documentation on public-facing APIs.** XML doc comments (`///`) on controller actions (they feed Swagger) and on public members of packages consumed by others, such as NuGet libraries.
- **Forbidden:** comments that restate the code, commented-out code, and change-log comments. Git keeps the history.

Tests are stricter: only the `// Arrange`, `// Act`, `// Assert` markers are allowed (see [TESTING.md](TESTING.md#test-style)).

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
5. **Check conformity.** New files must land in the correct layer and [feature folder](#feature-folders) and use the matching namespace.
6. **Register new types** in `RegisterDependencies.cs`.

## Commit Messages

Use [Conventional Commits](https://www.conventionalcommits.org/) for every commit and PR title:

```text
<type>(<optional scope>): <short summary in the imperative>
```

- Common types: `feat`, `fix`, `refactor`, `test`, `docs`, `build`, `ci`, `chore`.
- Mark breaking changes with `!` after the type (e.g. `feat!: Remove v1 endpoints`) or a `BREAKING CHANGE:` footer.
- Examples: `feat: Add GetFoo query`, `fix(api): Return 404 when Foo is missing`, `docs: Add clean code rules`.
