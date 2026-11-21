# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

This is a reference/template .NET 10 solution demonstrating Clean Architecture + CQRS + centralized IoC, built with strict TDD. The project rules live in the docs imported below; they are binding, not suggestions.

## Commands

```bash
dotnet build DotNetExample.slnx              # warnings and .editorconfig style errors fail the build
dotnet test DotNetExample.slnx
dotnet test --filter "FullyQualifiedName~Architecture"   # architecture rules only
dotnet test --filter "FullyQualifiedName~FooControllerTests.GetShould.ReturnOk"   # single test
dotnet run --project DotNetExample.IoC       # the IoC project is the executable
```

## Working Rules for Claude

- **TDD is mandatory.** Write a failing test, run it and confirm it fails, then write the minimum code to pass, then refactor without touching asserts. Never write implementation code first.
- **Small steps.** After every file change or test run, stop and evaluate the next step. Don't generate large amounts of code at once or anticipate future requirements.
- **Mock only wrappers.** When testing Commands/Queries, use the real repository with a mocked wrapper (e.g. `IHttpClientWrapper`).
- **Feature folders.** Put new code in a per-feature folder inside its layer (e.g. `Domain/Commands/Cars/`) with a matching namespace. Only controllers and shared wrappers stay flat.
- **One test file per method under test** (`GetShould.cs`). Before adding one, read the existing `<ClassName>TestBase` to avoid redefining `[TestInitialize]`.
- Register every new handler/repository/wrapper in `DotNetExample.IoC/RegisterDependencies.cs`.
- **New wrappers get contract tests**, in `DotNetExample.IntegrationTests` against the real dependency, never mocked (see [TESTING.md](TESTING.md#integration-tests)).
- **Never weaken enforcement to get green.** If a style error or an architecture test fails, fix the code. Don't lower an `.editorconfig` severity, add a `NoWarn`, or add an exemption to `Tests/Architecture/` unless the user agrees.
- **Use Conventional Commits** for commit messages and PR titles (see [CONTRIBUTING.md](CONTRIBUTING.md#commit-messages)).

## Reference Implementation

`CarsController` is the reference CRUD feature: copy its shape for new resources.

- Tests are integration tests over `WebApplicationFactory<Program>` (see `Tests/Api/CarsControllerTests/CarsControllerTestBase.cs`), so `[ApiController]` validation (400) and unhandled exceptions (500) are exercised for real. Only `IDocumentDatabaseWrapper` is replaced with a substitute.
- Storage goes through `IDocumentDatabaseWrapper` (named for the kind of store); `InMemoryDocumentDatabaseWrapper` is just the current implementation. See [Wrapper Interfaces](CONTRIBUTING.md#wrapper-interfaces).
- Commands return `Task`, so "not found" on update/delete is signalled with `CarNotFoundException` and mapped to 404 in the controller.
- FluentAssertions is pinned to 7.x: 8.x requires a commercial license.

## Known Gaps

The original `FooControllerTests` still uses MSTest `Assert` and mocks Domain handlers; it predates the current standards.

## Project Docs

@README.md
@CONTRIBUTING.md
@TESTING.md
@REVIEW.md
