---
name: dotnet-conventions
description: Set up a new .NET solution (or retrofit an existing one) with the DotNetExample conventions - Clean Architecture layers, CQRS handlers named as sentences, a centralized IoC composition root, wrapper interfaces at the infrastructure boundary, build-enforced .editorconfig style, architecture tests that enforce the rules, wrapper contract tests, strict TDD, and the agent/contributor docs that describe it all. Use when the user asks to create, scaffold, bootstrap or template a .NET / C# / ASP.NET Core project "like DotNetExample", "with our conventions", "with clean architecture and TDD", or to bring an existing .NET repo up to these standards.
---

# .NET Project Conventions

This skill reproduces the conventions of the DotNetExample reference solution in a new project. The rules themselves are written up in `assets/docs/` (README, CONTRIBUTING, TESTING, REVIEW, CLAUDE, AGENTS). Read **CONTRIBUTING.md and TESTING.md before you start**. They're the specification; this file is the setup procedure.

## What gets set up

| Piece | Source in this skill | Purpose |
|---|---|---|
| SDK pin | `assets/global.json` | Same compiler and analyzers on every machine |
| Shared build settings | `assets/Directory.Build.props` | `TreatWarningsAsErrors`, `EnforceCodeStyleInBuild`, nullable, implicit usings |
| Style rules | `assets/editorconfig` (rename to `.editorconfig`) | `var`, braces, file-scoped namespaces, `_camelCase` fields, namespace = folder… as build errors |
| Layered projects | `assets/projects/*` | Domain ← DataAccess, Domain ← Api, IoC composes all; two test projects |
| Architecture tests | `assets/architecture/*.cs.template` | Fail the build when a convention is broken |
| Reference feature | `assets/reference/**` | The complete Cars CRUD feature, every file in every layer: entities, handlers, repository, wrappers, controller, API models, controller tests, wrapper contract tests |
| Docs | `assets/docs/*.md`, `assets/docs/CLAUDE.md.template` | Binding rules for humans and agents |
| CI | `assets/ci/build.yml` | GitHub Actions: build and test on PRs, `main`, and weekly |

Files under `assets/` keep the original `DotNetExample` names. `.cs` files are stored as `.cs.template` so the source-parsing architecture test in a host repo doesn't pick them up, and `CLAUDE.md` is stored as `CLAUDE.md.template` so Claude Code doesn't load it as a nested `CLAUDE.md` while working in this folder. When copying, drop the `.template` suffix and replace names as described below.

Everything in `assets/` except `ci/` is a generated copy of the DotNetExample repository (see [Maintaining this skill](#maintaining-this-skill)). `reference/` is the whole Cars feature, so its files compile together; it leaves out only DotNetExample's legacy Foo example, and `RegisterDependencies.cs.template` has the Foo lines removed.

## Step 0: Gather inputs

Before writing anything, settle these with the user (ask only for what you can't infer):

1. **Solution name**, e.g. `Acme.Orders`. Every `DotNetExample` becomes this name, in file names, namespaces, project references and docs.
2. **Kind of app.** The template is an ASP.NET Core Web API. For a worker, CLI or library, keep Domain, DataAccess, IoC and the tests, and drop the Api project and the API sections of the docs. The IoC project stays the executable (a `Microsoft.NET.Sdk.Worker` host, for instance). See [Apps without an API](#apps-without-an-api) for the matching edits to the test projects and architecture tests.
3. **The kinds of store** the first features need (document DB, SQL, HTTP API, clock…). Each becomes a wrapper interface named after the *kind*, never the product. See CONTRIBUTING "Wrapper Interfaces".
4. **SDK and target framework.** Run `dotnet --list-sdks`. Pin `global.json` to an installed feature band (`x.y.z00`, `rollForward: latestPatch`, `allowPrerelease: false`). Target the newest LTS runtime unless the user wants otherwise. Don't set `LangVersion`.
5. **Where it lives.** A new directory with `git init`, or an existing repo (see [Retrofitting](#retrofitting-an-existing-project)).

## Step 1: Repository and build configuration

1. `git init`, then `dotnet new gitignore`.
2. Copy `global.json`, `Directory.Build.props` and `editorconfig` (as `.editorconfig`) to the repo root. Adjust `TargetFramework` and the SDK version from Step 0. Keep the `GenerateDocumentationFile` + `NoWarn CS1591` pair: IDE0005 (unused usings) only runs during build when documentation generation is on, and the comment says why.
3. Don't add other `NoWarn` entries or lower `.editorconfig` severities. Every silencing needs a *why* comment and the user's agreement.

## Step 2: Projects and references

Create the solution and projects (replace `MyApp` with the solution name):

```bash
dotnet new sln --format slnx -n MyApp
dotnet new classlib -n MyApp.Domain
dotnet new classlib -n MyApp.DataAccess
dotnet new classlib -n MyApp.Api
dotnet new web -n MyApp.IoC
dotnet new mstest -n MyApp.Tests
dotnet new mstest -n MyApp.IntegrationTests
dotnet sln MyApp.slnx add MyApp.Domain MyApp.DataAccess MyApp.Api MyApp.IoC MyApp.Tests MyApp.IntegrationTests
```

For an app without an API, leave out `MyApp.Api` and create the IoC project with `dotnet new worker` (or `console`) instead of `web`.

Then make each `.csproj` match `assets/projects/`:

- **Remove everything `Directory.Build.props` already sets** (`TargetFramework`, `Nullable`, `ImplicitUsings`). A project file holds only its SDK, references and packages.
- **Domain** references nothing.
- **DataAccess** → Domain, plus infrastructure packages (e.g. `Microsoft.Extensions.Http`, Dapper, a MongoDB driver).
- **Api** is a plain `Microsoft.NET.Sdk` class library with `<FrameworkReference Include="Microsoft.AspNetCore.App" />` → Domain. **No `Program.cs`.**
- **IoC** is `Microsoft.NET.Sdk.Web` → Api, DataAccess, Domain, plus Swashbuckle.
- **Tests** uses `MSTest.Sdk` → every production project, with FluentAssertions, NSubstitute, RandomTestValues, `TngTech.ArchUnitNET`, `Microsoft.CodeAnalysis.CSharp`, `Microsoft.AspNetCore.Mvc.Testing`. Keep the `NoWarn CS8618` with its comment: fields set in `[TestInitialize]` are invisible to nullable analysis.
- **IntegrationTests** uses `MSTest.Sdk` → DataAccess only, with FluentAssertions and RandomTestValues (+ `FrameworkReference` to ASP.NET Core if it hosts a local HTTP server, + Testcontainers for real databases).
- Both test projects get `MSTestSettings.cs`: `[assembly: Parallelize(Scope = ExecutionScope.MethodLevel)]`.

Package versions: add with `dotnet add package <Name>` to get the current stable release rather than copying the versions in `assets/projects/`. **Exception: pin FluentAssertions to the latest 7.x** (`--version 7.*`, then fix the resolved version). 8.x requires a commercial license.

Delete the template leftovers: `Class1.cs`, the `mstest` sample test, and the `web` template's minimal-API `Program.cs` body.

## Step 3: Composition root

Copy from `assets/reference/DotNetExample.IoC/` and `DotNetExample.Api/ApiConfiguration.cs.template`:

- `Program.cs` declares `public partial class Program` with an explicit `Main`, so `WebApplicationFactory<Program>` can reach it from tests.
- `ApiConfiguration.AddApi()` is the Api layer's extension method (controllers, `System.Text.Json` options such as `JsonStringEnumConverter`).
- `RegisterDependencies.Register(services)` is the single place handlers, repositories and wrappers get registered. The template shows the Cars feature's registrations, as an example of their shape. Start the new project's file empty, with no Cars lines: each feature adds its own.

Run `dotnet build MyApp.slnx`. It must pass with zero warnings before you go on.

## Step 4: Architecture tests

Copy `assets/architecture/*.cs.template` into `MyApp.Tests/Architecture/` (except `WrappersShould`, which goes to `MyApp.IntegrationTests/Architecture/`). Then:

1. Replace `DotNetExample` with the solution name everywhere, including `SourceFiles.SolutionFileName`. `ProjectFiles` finds the projects through that solution file, so it must name the real `.slnx`.
2. **Fix the anchors in `SolutionAssemblies`.** Each assembly is found through one of its types (`typeof(Car).Assembly`). Point each at a type that exists in that layer: `ApiConfiguration` for Api, `Program` for IoC, a wrapper interface for DataAccess. If a layer has no types yet (Domain usually doesn't), use `Assembly.Load("MyApp.Domain")` until the first entity exists, then switch to a `typeof`. Do the same for the anchor in `WrappersShould`.
3. Adjust the lists to the project: `FeatureFoldersShould._featureFolderRoots`, `InfrastructureShould._infrastructureNamespaces` (add the infrastructure packages you depend on), `CodeShould._bannedNamespaces` (also used as the banned package IDs). For a non-API app, make the edits in [Apps without an API](#apps-without-an-api).
4. **Prove every rule can fail before trusting it.** With no features yet, most rules pass trivially (no handlers means `HandlersShould` checks nothing). Plant one violation per rule, run `dotnet test --filter "FullyQualifiedName~Architecture"`, confirm the failure message names the offender, then remove it.

   A reference that no code uses never reaches the compiled assembly, so rules that read assemblies need a plant that *uses* a type. Rules that read `.csproj` files need only the reference:

| Rule | Plant |
|---|---|
| `LayersShould.DeclareProjectReferencesOnlyInward` | Add a `ProjectReference` from Api to DataAccess |
| `LayersShould.KeepApiIndependentOfDataAccessAndIoC` | With that reference in place, give an Api class a property of a DataAccess type (e.g. `IDocumentDatabaseWrapper`) |
| `LayersShould.KeepDomainFreeOfPackageReferences` | Add any `PackageReference` to Domain |
| `LayersShould.KeepDomainFreeOfDependencies` | Once Domain has types: reference a non-`System` package from Domain and use one of its types there |
| `InfrastructureShould` | Use `System.IO.File` in a Domain type |
| `CodeShould.NotReferenceBannedPackages` | Add a `Newtonsoft.Json` `PackageReference` to any project |
| `CodeShould.NotUseBannedLibraries` | With that package in place, call `JsonConvert.SerializeObject(1)` from any production class |
| `CodeShould.NotUseAnonymousTypes` | `var x = new { A = 1 };` |
| `CodeShould.KeepLambdasToOneLineExpressions` | A block-bodied lambda |
| `CodeShould.NameAsyncMethodsWithAsyncSuffix` | `public Task Go()` |
| `FeatureFoldersShould` | A class directly in `MyApp.Domain.Commands` |
| `ApiShould` | A controller action returning a Domain type |
| `HandlersShould` | A handler with two methods, or a method called `ExecuteAsync` |
| `WrappersShould` | A wrapper class without a `<Name>Should` test class |

Report the planted violations and their failure messages to the user. That's the coverage-test rule from TESTING.md applied to the rules themselves.

## Step 5: Docs

Copy `assets/docs/*.md` to the repo root and adapt them. They're binding project rules, so keep them accurate:

- Rename `DotNetExample` throughout. Update the README layout tree, the project table and the commands.
- **CLAUDE.md:** copy `CLAUDE.md.template` to `CLAUDE.md`. Keep the Working Rules verbatim. Replace "Reference Implementation" with a pointer to the first real feature once it exists (until then, point to the shapes in CONTRIBUTING). **Delete "Known Gaps"**: Foo is specific to DotNetExample.
- **README.md:** replace "This repository is an example, so it deliberately has no CI pipeline" with a description of the real pipeline.
- Rewrite Cars-specific examples to use the new domain only where the example would otherwise confuse. Keep the † convention for names that don't exist in the repo, and mark Cars names † if you keep them.
- Drop sections that don't apply (API Models for a non-API app, unused kinds of store) rather than leaving them stale.
- Keep `AGENTS.md` as a pointer to `CLAUDE.md` so other agent tools follow the same rules.

## Step 6: CI

A real project needs CI **before its first feature**. Copy `assets/ci/build.yml` to `.github/workflows/build.yml` (or translate it for the user's CI system), rename the solution and test project names, and set `dotnet-version` to the target runtime: the pinned SDK builds the code, but the tests need the target runtime to run. Tell the user to make the job a **required status check** in branch protection. The weekly schedule catches newly published NuGet advisories, which `TreatWarningsAsErrors` turns into build errors.

## Step 7: First feature, by TDD

Don't copy the Cars feature in. Build the user's first real feature with the Red/Green/Refactor cycle in TESTING.md, using `assets/reference/` as the shape to follow:

- Controller test base over `WebApplicationFactory<Program>` that swaps **only the wrapper** for an NSubstitute substitute (`CarsControllerTestBase.cs.template`).
- One `<MethodName>Should.cs` per controller action, scenarios named `<Outcome>_<Condition>`, `// Arrange // Act // Assert`, `RandomTestValues` data, mocks matching the exact collection/id/URI/SQL.
- Handlers `VerbNoun` with their own `IVerbNoun` interface and one sentence-completing method (`ByAsync`, `WithAsync`, `AllAsync`). Commands return `Task` and signal not-found with a domain exception; Queries return `Task<T>`.
- Real repository in DataAccess building the raw query, calling the wrapper.
- API-only `Request`/`Response` records with `request.ToX(id)` and `XResponse.From(x)`, API-owned enums mapped by `switch`.
- Feature folders in every layer; controllers and wrappers stay flat.
- Each new wrapper gets a `<Interface>Contract` + `<Implementation>Should` in IntegrationTests, against the real dependency (in-memory instance, local Kestrel server, or Testcontainers).
- Register everything in `RegisterDependencies.cs`.
- Cover 200/201/204, 400, 404 and 500 where they apply.

Work in small steps: one failing test, minimum code, refactor, then stop and decide the next step. Name every test that passed on its first run and how you proved it can fail.

## Step 8: Verify and hand off

- `dotnet build MyApp.slnx` passes with no warnings.
- `dotnet test MyApp.slnx` passes, architecture tests included.
- Walk REVIEW.md's checklist over the result.
- Propose a Conventional Commit (`build: Scaffold solution with project conventions`), but commit only if the user asks.

## Retrofitting an existing project

Introduce the pieces in an order that keeps the build green at each step, and agree the order with the user first:

1. `global.json` and `Directory.Build.props` without `TreatWarningsAsErrors`. Fix the warnings, then turn it on.
2. `.editorconfig`. Run `dotnet format` for the auto-fixable rules. Fix the naming rules (IDE1006) by hand.
3. Architecture tests, one file at a time. Fix the violations each one reports. Don't add exemptions without the user's agreement, and if one is agreed, give it a *why* comment.
4. Docs and CI as in Steps 5–6.
5. Move code into layers and feature folders, and existing tests toward the mocking rules, feature by feature, as the code is touched.

## Apps without an API

The architecture tests assume an Api layer and ASP.NET Core. For a worker, CLI or library, make these edits as well as dropping the Api project, or the Tests project won't build:

| File | Edit |
|---|---|
| `Tests.csproj` | Remove the Api `ProjectReference` and the `Microsoft.AspNetCore.Mvc.Testing` package |
| `IoC.csproj` | `Microsoft.NET.Sdk.Worker` (or `Microsoft.NET.Sdk` with `OutputType` `Exe`) → DataAccess, Domain; no Swashbuckle |
| `Program.cs` | Build a `Host.CreateApplicationBuilder(args)` host and call `RegisterDependencies.Register(builder.Services)`; no `AddApi()`, Swagger or controllers |
| `SolutionAssemblies` | Remove `using MyApp.Api;`, the `Api` field, and `Api` from `Production` |
| `LayersShould` | Delete `KeepApiIndependentOfDataAccessAndIoC`; change `KeepDataAccessIndependentOfApiAndIoC` to check IoC only (rename it `KeepDataAccessIndependentOfIoC`); in `AllowedProjectReferences`, remove the Api entry and Api from IoC's list |
| `InfrastructureShould` | Remove `SolutionAssemblies.Api` from `layersThatMustUseWrappers` |
| `CodeShould` | Remove `using Microsoft.AspNetCore.Mvc;`, `IsController` and its call in `AsyncMethodsWithoutSuffixIn` (the exemption exists only for controller actions) |
| `FeatureFoldersShould` | Remove the `MyApp.Api.Models` root |
| `ApiShould` | Don't copy it |

The controller tests in `reference/DotNetExample.Tests/Api/` don't apply. Test the app's entry point (a hosted service, a command handler) with the real handlers and repositories over substituted wrappers, the same way the controller tests do.

## Maintaining this skill

This skill lives inside the DotNetExample repository and copies it: when the repo changes, the skill has to change with it. Follow [MAINTAINING.md](MAINTAINING.md) after any change to the repo's code, docs, build settings or architecture tests. In short: run `scripts/sync-assets.sh` to regenerate `assets/` from the repo, then update this file wherever the steps, tables or rule names no longer match.
