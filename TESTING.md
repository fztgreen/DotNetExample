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

## File Organization & Naming

- Pattern: `DotNetExample.Tests/<Namespace>/<ClassName>Tests/<MethodName>Should.cs`
- **One file per method under test.** The file holds one `[TestClass]` named `<MethodName>Should`, and every scenario for that method is a test method in it, named `<Outcome>_<Condition>`. Together they read as a sentence: `GetShould.ReturnNotFound_WhenCarDoesNotExist`.
- Examples:
  - `Api/CarsControllerTests/GetShould.cs` → `ReturnOk_WhenCarExists`, `ReturnNotFound_WhenCarDoesNotExist`, `ReturnInternalServerError_WhenDatabaseFails`
  - `Api/CarsControllerTests/CreateShould.cs` → `ReturnCreated_AndStoreCar`, `ReturnBadRequest_WhenRequestIsInvalid`
- Share setup across a class's method files with an abstract `<ClassName>TestBase` in the same folder (e.g. `CarsControllerTestBase.cs`) that holds `[TestInitialize]`, `[TestCleanup]`, and test-data builders. Each `<MethodName>Should` class inherits from it.
- **Before adding a new test file, read the existing test base class first** so you don't redefine `[TestInitialize]` or duplicate setup logic.
