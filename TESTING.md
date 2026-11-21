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
- **ArchUnitNET** for architecture tests that need type-level dependencies
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

Mock as little as possible. Put the mock at the lowest point that has to be faked: the call that actually leaves the process (network, database, clock, file system). Everything above that point runs for real in the test: query building, parameter mapping, response parsing.

- **Allowed:** mocking infrastructure wrappers (`IHttpClientWrapper`, `IDocumentDatabaseWrapper`, `ISqlDatabaseWrapper`, `IDateTimeWrapper`, …), and only at the method that performs the external call.
- **Forbidden:** mocking Domain repository interfaces (e.g. `IFooRepository`) in logic tests. When testing Commands/Queries, use the **real** repository injected with a **mocked** wrapper.
- **Forbidden:** mocking domain entities or value objects. Use real instances.

### Mock at the boundary, not above it

- A wrapper is a thin pass-through to the driver. It takes the final value that goes to the external system (the full request URI, the SQL text, the stored procedure name and its parameters, the document collection and id or filter) and contains no logic of its own.
- The wrapper interface is named after the kind of store (`IDocumentDatabaseWrapper`), not the technology behind it (`InMemory…`, `Mongo…`), so the same tests keep working when the implementation changes. See [Wrapper Interfaces](CONTRIBUTING.md#wrapper-interfaces).
- The repository builds that value. Because the test uses the real repository, the query is exercised, not hidden.
- Set the mock to respond **only to the exact expected value**, e.g. `wrapper.GetAsync($"foo/{id}").Returns(json)`, not `Arg.Any<string>()`. If the repository builds the wrong URI or query, the mock returns nothing and the test fails.
- **Forbidden:** wrapper methods that take domain intent instead of the raw call, e.g. `GetFooAsync(id)` or `ExecuteGetFooProc(id)`. They move the query out of reach of tests.
- **Forbidden:** `Arg.Any<T>()` for the value that identifies the call (URI, query, procedure name). Use it only for arguments that don't matter to the behavior under test, like a `CancellationToken`.

## Assertions

- Assert on return values or state changes whenever possible.
- Use `Received()` interaction checks only for void methods where no return value or state change can be observed (e.g. logging, or the final boundary call).

## Test Style

- **Required:** `// Arrange`, `// Act`, `// Assert` comments delimiting each phase.
- **Forbidden:** other inline comments explaining what or why. Extract logic into well-named private methods instead.
- **Forbidden:** anonymous types, and lambdas longer than a single one-line expression, including in test data (see [No Anonymous Types; Keep Lambdas Short](CONTRIBUTING.md#no-anonymous-types-keep-lambdas-short)). Short lambdas such as `Arg.Is<Car>(car => car.Id == id)` are fine. For data-driven cases, either is fine: short lambdas (`("Make empty", request => request with { Make = "" })`) or named methods passed by name (`("Make empty", WithEmptyMake)`).

## File Organization & Naming

- Pattern: `DotNetExample.Tests/<Namespace>/<ClassName>Tests/<MethodName>Should.cs`
- **One file per method under test.** The file holds one `[TestClass]` named `<MethodName>Should`, and every scenario for that method is a test method in it, named `<Outcome>_<Condition>`. Together they read as a sentence: `GetShould.ReturnNotFound_WhenCarDoesNotExist`.
- Examples:
  - `Api/CarsControllerTests/GetShould.cs` → `ReturnOk_WhenCarExists`, `ReturnNotFound_WhenCarDoesNotExist`, `ReturnInternalServerError_WhenDatabaseFails`
  - `Api/CarsControllerTests/CreateShould.cs` → `ReturnCreated_AndStoreCar`, `ReturnBadRequest_WhenRequestIsInvalid`
- Share setup across a class's method files with an abstract `<ClassName>TestBase` in the same folder (e.g. `CarsControllerTestBase.cs`) that holds `[TestInitialize]`, `[TestCleanup]`, and test-data builders. Each `<MethodName>Should` class inherits from it.
- **Before adding a new test file, read the existing test base class first** so you don't redefine `[TestInitialize]` or duplicate setup logic.

## Architecture Tests

`DotNetExample.Tests/Architecture/` holds tests that enforce the conventions in [CONTRIBUTING.md](CONTRIBUTING.md) that a style analyzer can't see. They read the compiled assemblies, so they check what the code actually does, not just how it looks.

| File | Enforces |
|---|---|
| `LayersShould.cs` | Project references between layers |
| `InfrastructureShould.cs` | Only `DataAccess/Wrappers` depends on infrastructure namespaces (uses ArchUnitNET to read method bodies) |
| `CodeShould.cs` | Banned libraries, no anonymous types, lambdas limited to one-line expressions, `Async` suffix |
| `FeatureFoldersShould.cs` | No types directly in a feature folder root |
| `HandlersShould.cs` | Handler interface, method count, return types, and method naming |

- Files are named `<Subject>Should.cs`, with one test per rule, so a failure reads as a sentence: `HandlersShould.ImplementTheirOwnInterface`. This is the one exception to the `<ClassName>Tests/<MethodName>Should.cs` pattern, because there's no single class under test.
- Each test collects *every* violation and asserts `ViolationReport.Of(violations).Should().BeEmpty()`, so one run reports all offending types or methods by name. Don't assert `violations.Should().BeEmpty()` directly: on a list, FluentAssertions names only the first offender.
- **When you add a convention that can be checked mechanically, add a rule here.** Before trusting it, prove it can fail: plant a violation, watch the test fail with a message that names it, then remove the violation. A rule that has never failed may not be checking anything.
- Anonymous types are detected in the compiled assemblies, by the names the compiler generates for them (`<>f__AnonymousType…`). Lambda length can't be seen after compilation, so `CodeShould.KeepLambdasToOneLineExpressions` parses the source files with Roslyn (`Microsoft.CodeAnalysis.CSharp`). It flags block-bodied lambdas, lambdas that span lines, and `delegate { … }` blocks, and reports each by file and line.
