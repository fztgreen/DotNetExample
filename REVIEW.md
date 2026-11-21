# Code Review Checklist

Use this checklist when reviewing changes. Each item traces back to a rule in [CONTRIBUTING.md](CONTRIBUTING.md) or [TESTING.md](TESTING.md).

## Architecture

- [ ] Domain has no new project or infrastructure package references.
- [ ] Api does not reference DataAccess.
- [ ] No `Program.cs` or DI wiring outside the IoC project.
- [ ] New handlers, repositories, and wrappers are registered in `RegisterDependencies.cs`.
- [ ] Repositories reach external stores only through a wrapper interface.
- [ ] New files are in the correct layer and folder, with matching namespaces.

## Design & Conventions

- [ ] Writes are Commands in `Domain/Commands` returning `Task`; reads are Queries in `Domain/Queries` returning `Task<T>`.
- [ ] Handlers are named `VerbNoun`, implement their own interface, and take repository interfaces through the constructor.
- [ ] Interfaces are prefixed with `I`; async methods end with `Async`.
- [ ] Functions are small and names are descriptive; code reads without explanation.
- [ ] Comments only explain *why*, or document public-facing APIs; no commented-out code.
- [ ] New projects enable `Nullable`, `ImplicitUsings`, and `TreatWarningsAsErrors`.
- [ ] The change solves the current requirement only, with no speculative code.

## Tests

- [ ] Every behavior change has a test that was written first and seen to fail.
- [ ] Applicable status codes are covered: 200/201/204, 400, 404, 500.
- [ ] Only infrastructure wrappers are mocked. Repositories, entities, and value objects are real.
- [ ] Mocks match the exact URI/query/procedure the code should send; no logic lives in wrappers.
- [ ] Assertions check return values or state; `Received()` is used only where nothing else is observable.
- [ ] Test data is randomized with `RandomTestValues`, not hardcoded.
- [ ] Tests have `// Arrange`, `// Act`, `// Assert` comments and no other explanatory comments.
- [ ] Test files follow `<Namespace>/<ClassName>Tests/<MethodName>_<Scenario>.cs`, and partial classes don't duplicate setup.
- [ ] Refactor-only changes leave existing Assert statements untouched.

## Build

- [ ] `dotnet build DotNetExample.slnx` passes with no warnings.
- [ ] `dotnet test DotNetExample.slnx` passes.
