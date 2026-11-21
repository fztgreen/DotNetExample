# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

This is a reference/template .NET 10 solution demonstrating Clean Architecture + CQRS + centralized IoC, built with strict TDD. The project rules live in the docs imported below; they are binding, not suggestions.

## Commands

```bash
dotnet build DotNetExample.slnx              # warnings are errors in every project
dotnet test DotNetExample.slnx
dotnet test --filter "FullyQualifiedName~FooControllerTests.GetShould.ReturnOk"   # single test
dotnet run --project DotNetExample.IoC       # the IoC project is the executable
```

## Working Rules for Claude

- **TDD is mandatory.** Write a failing test, run it and confirm it fails, then write the minimum code to pass, then refactor without touching asserts. Never write implementation code first.
- **Small steps.** After every file change or test run, stop and evaluate the next step. Don't generate large amounts of code at once or anticipate future requirements.
- **Mock only wrappers.** When testing Commands/Queries, use the real repository with a mocked wrapper (e.g. `IHttpClientWrapper`).
- **Before adding a partial test file, read the existing base/setup partial** to avoid redefining `[TestInitialize]`.
- Register every new handler/repository/wrapper in `DotNetExample.IoC/RegisterDependencies.cs`.
- **Use Conventional Commits** for commit messages and PR titles (see [CONTRIBUTING.md](CONTRIBUTING.md#commit-messages)).

## Reference Implementation

`CarsController` is the reference CRUD feature: copy its shape for new resources.

- Tests are integration tests over `WebApplicationFactory<Program>` (see `Tests/Api/CarsControllerTests/CarsControllerTestBase.cs`), so `[ApiController]` validation (400) and unhandled exceptions (500) are exercised for real. Only `IInMemoryDatabaseWrapper` is replaced with a substitute.
- Commands return `Task`, so "not found" on update/delete is signalled with `CarNotFoundException` and mapped to 404 in the controller.
- FluentAssertions is pinned to 7.x: 8.x requires a commercial license.

## Known Gaps

The original `FooControllerTests` still uses MSTest `Assert` and mocks Domain handlers; it predates the current standards.

## Project Docs

@README.md
@CONTRIBUTING.md
@TESTING.md
@REVIEW.md
