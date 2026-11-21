# Testing

## Running Tests

```bash
dotnet test DotNetExample.slnx
dotnet test --filter "FullyQualifiedName~FooControllerTests.GetShould.ReturnOk"   # single test
```

Tests run in parallel at method level (`MSTestSettings.cs`), so tests must not share mutable state.

## Stack

- **MSTest** via `MSTest.Sdk`
- **NSubstitute** for mocking
- **FluentAssertions** for assertions
- **RandomTestValues** for randomized test data
- **Microsoft.AspNetCore.App** framework reference for API integration tests. `Program` is declared `public partial class Program` in the IoC project so `WebApplicationFactory<Program>` can reach it.

## Test Driven Development

Every change follows the **Red/Green/Refactor** cycle. Implementation code may not be written without a verified failing test.

### 1. Red — write a failing test

- Create the test file following the [naming rules](#file-organization--naming).
- Use randomized data from `RandomTestValues` instead of constants, so hardcoded implementations (e.g. `return 0`) can't pass by accident.
- Run it with `dotnet test --filter <TestName>`. It **must fail**. If the code doesn't exist yet, add stubs that throw `NotImplementedException` so it compiles, then confirm the failure.

### 2. Green — simplest implementation

- Write the minimum code needed to pass. Don't implement the full algorithm yet; if `Add(0,0)` expects `0`, return `0`. This forces the next test case.
- Run the test. It must pass.

### 3. Refactor — improve structure only

- **Refactoring must never change a test's Assert statements.** If an assert has to change, you're in a new Red phase.
- Use expressive names, remove magic strings, extract methods, and use `TestBase` classes to reduce setup boilerplate.

### 4. Iterate

Add the next test case to evolve the logic. Use `[DataRow]` to add permutations to existing tests efficiently.

## Coverage Requirements

Every feature must cover these scenarios where applicable:

| Path | Status | Scenario |
|---|---|---|
| Happy | 200 OK | Request succeeded with data |
| Happy | 201 Created | Resource created |
| Happy | 204 No Content | Update/Delete succeeded |
| Unhappy | 400 Bad Request | Validation failure (invalid model, missing fields) |
| Unhappy | 404 Not Found | Resource doesn't exist (Get/Update/Delete) |
| Unhappy | 500 Internal Server Error | Unexpected downstream failure (exception thrown from a mocked wrapper) |

## Mocking Rules

- **Allowed:** mocking infrastructure wrappers (`IHttpClientWrapper`, `IMongoDbWrapper`, `IDateTimeWrapper`, …).
- **Forbidden:** mocking Domain repository interfaces (e.g. `IFooRepository`) in logic tests. When testing Commands/Queries, use the **real** repository injected with a **mocked** wrapper.
- **Forbidden:** mocking domain entities or value objects. Use real instances.

## Assertions

- Assert on return values or state changes whenever possible.
- Use `Received()` interaction checks only for void methods where no return value or state change can be observed (e.g. logging, or the final boundary call).

## Test Style

- **Required:** `// Arrange`, `// Act`, `// Assert` comments delimiting each phase.
- **Forbidden:** other inline comments explaining what or why. Extract logic into well-named private methods instead.

## File Organization & Naming

- Pattern: `DotNetExample.Tests/<Namespace>/<ClassName>Tests/<MethodName>_<Scenario>.cs`
- Examples:
  - `Api/FooControllerTests/GetShould_ReturnOk.cs`
  - `Api/FooControllerTests/GetShould_Throw_WhenInvalid.cs`
- Use `partial class` to share setup when splitting tests for the same method across files.
- **Before adding a new partial file, read the existing base/setup partial first** so you don't redefine `[TestInitialize]` or duplicate setup logic.
