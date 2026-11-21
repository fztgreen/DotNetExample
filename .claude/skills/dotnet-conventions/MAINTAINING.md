# Maintaining the dotnet-conventions skill

This skill is a copy of the DotNetExample repository it lives in (`../../..` from here). The repository is the source of truth: its code compiles, its tests run, and its docs are the binding rules. The skill can't be built or tested where it is, so keep it a faithful copy and change the repository first.

Do this after **any** change to the repository's code, docs, build settings, project files or architecture tests, in the same branch as that change.

## What is copied, and what isn't

`scripts/sync-assets.sh` generates everything in `assets/` except `ci/`:

| Repository | `assets/` |
|---|---|
| `global.json`, `Directory.Build.props` | same name |
| `.editorconfig` | `editorconfig` |
| `README.md`, `CONTRIBUTING.md`, `TESTING.md`, `REVIEW.md`, `AGENTS.md` | `docs/` |
| `CLAUDE.md` | `docs/CLAUDE.md.template`, so Claude Code doesn't load it as a nested `CLAUDE.md` |
| `DotNetExample.slnx`, every `DotNetExample.*/DotNetExample.*.csproj` | `projects/` |
| `DotNetExample.Tests/MSTestSettings.cs` | `projects/MSTestSettings.cs.template` (the script warns if the IntegrationTests copy differs) |
| `DotNetExample.Tests/Architecture/*.cs`, `DotNetExample.IntegrationTests/Architecture/*.cs` | `architecture/<name>.cs.template` |
| Every other tracked `DotNetExample.*/**/*.cs` | `reference/<same path>.cs.template` |

Left out or changed on purpose:

- **Foo.** Any path containing `Foo` is skipped, and lines containing `Foo` are removed from `RegisterDependencies.cs`. Foo is the repository's legacy example (CLAUDE.md "Known Gaps") and isn't a shape to copy.
- **`ci/build.yml`** exists only in the skill, because the repository deliberately has no CI. Edit it by hand, with CRLF line endings.
- **`SKILL.md`, this file and `scripts/`** are written by hand.

The script writes every file with CRLF line endings, because the bundled `.editorconfig` requires them and Git Bash tools on Windows (`sed -i`, heredocs) silently write LF.

**Never edit a generated file in `assets/` by hand.** The next sync overwrites it. Fix the repository file instead, then sync.

## Procedure

1. **Finish the change in the repository.** Follow CLAUDE.md as usual: TDD, docs updated, `dotnet build DotNetExample.slnx` with no warnings, `dotnet test DotNetExample.slnx` green. For a new or changed architecture rule, plant a violation **in the repository**, watch the rule fail, and remove the plant. That's the only place the rule can actually run.
2. **Regenerate the assets**, from the repository root (Git Bash on Windows):

   ```bash
   bash .claude/skills/dotnet-conventions/scripts/sync-assets.sh
   ```

   It prints each file it `updated` or `removed`. Read that list: it's what step 3 has to account for.
3. **Update `SKILL.md`** for each kind of change in the list:

   | Change in the repository | Update in `SKILL.md` |
   |---|---|
   | Architecture rule added, renamed or removed | Step 4's plant table: one row per rule, with a plant you've seen fail in step 1. A rule that reads compiled assemblies needs a plant that *uses* a type, because an unused reference isn't compiled in. A rule that reads `.csproj` files needs only the reference. |
   | Architecture rule or helper that depends on the Api layer or ASP.NET Core | The [Apps without an API](SKILL.md#apps-without-an-api) table |
   | A list inside a rule that each project must tailor (namespaces, folder roots, banned libraries) | Step 4, item 3 |
   | New anchor type in `SolutionAssemblies` or `WrappersShould` | Step 4, item 2 |
   | Project added or removed, or packages changed | Step 2: the `dotnet new` / `dotnet sln add` commands and the per-project bullets |
   | `global.json`, `Directory.Build.props` or `.editorconfig` | Step 1; and the SDK or runtime version in `assets/ci/build.yml` |
   | Doc section that's specific to DotNetExample (like "Known Gaps") | Step 5, so the agent knows to delete or adapt it |
   | New feature beyond Cars | Decide whether it's a shape worth copying. If not, exclude it in `sync-assets.sh` the way Foo is excluded. Update the "What gets set up" table either way. |
   | Cars file or type renamed (e.g. `CarsControllerTestBase`) | Every place `SKILL.md` names it, mostly Steps 3 and 7 |
   | Foo removed, or `RegisterDependencies.cs` reorganized | The `without_foo` filter in `sync-assets.sh`, and the Foo notes in this file and `SKILL.md` |
   | New convention in CONTRIBUTING or TESTING | Step 7, if the first feature has to follow it |

   If a change doesn't fit a row, read `SKILL.md` top to bottom and fix whatever it now gets wrong. Then add the row here.
4. **Verify.**

   ```bash
   bash .claude/skills/dotnet-conventions/scripts/sync-assets.sh --check
   ```

   This must print `assets/ matches the repo.` Then check that every rule and file `SKILL.md` names still exists. This lists the rule names it mentions, to compare with `DotNetExample.Tests/Architecture/` and `DotNetExample.IntegrationTests/Architecture/`:

   ```bash
   grep -oE '[A-Z][A-Za-z]+Should(\.[A-Za-z_]+)?' .claude/skills/dotnet-conventions/SKILL.md | sort -u
   ```

5. **Commit the skill with the repository change**, in the same commit or the same pull request, so `main` never has a skill that describes an older repository. A skill-only commit uses the `skill` scope, e.g. `docs(skill): Add plant for NotReferenceBannedPackages`.

## Changing the skill itself

To change how the skill sets up a project, without changing the repository:

- Steps and wording go in `SKILL.md`.
- If the new step needs a file the repository doesn't have, it's probably a sign the repository should have it too: add it there first. Only CI-style files the repository deliberately lacks belong in `assets/` as hand-written files, and they must be added to the `ci/*` skip in `sync-assets.sh`, or the next sync deletes them as stale.
