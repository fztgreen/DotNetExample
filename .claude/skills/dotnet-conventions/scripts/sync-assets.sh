#!/usr/bin/env bash
# Regenerates assets/ from the DotNetExample repository this skill lives in.
#
#   scripts/sync-assets.sh          copy every mirrored file from the repo into assets/
#   scripts/sync-assets.sh --check  report drift only; exits 1 if assets/ is out of date
#
# Every file is written with CRLF line endings, because the bundled .editorconfig
# requires them and Git Bash tools on Windows silently write LF.
# MAINTAINING.md lists what is mirrored and what is deliberately left out.

set -euo pipefail

skill_dir="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
repo_root="$(cd "$skill_dir/../../.." && pwd)"
assets="$skill_dir/assets"
check_only=false
[[ "${1:-}" == "--check" ]] && check_only=true

drift=0
declare -A expected_assets=()

to_crlf() {
    sed 's/\r$//; s/$/\r/'
}

# Foo is DotNetExample's legacy example (see CLAUDE.md "Known Gaps") and isn't shipped.
without_foo() {
    grep -v 'Foo'
}

mirror() {
    local source="$1" target="$2" filter="${3:-cat}"
    expected_assets["$target"]=1
    local rendered
    rendered="$($filter < "$repo_root/$source" | to_crlf; printf x)"
    rendered="${rendered%x}"

    if [[ -f "$assets/$target" ]] && [[ "$(cat "$assets/$target"; printf x)" == "${rendered}x" ]]; then
        return
    fi

    drift=1
    if $check_only; then
        echo "out of date: assets/$target (from $source)"
    else
        mkdir -p "$(dirname "$assets/$target")"
        printf '%s' "$rendered" > "$assets/$target"
        echo "updated: assets/$target"
    fi
}

cd "$repo_root"

mirror global.json global.json
mirror Directory.Build.props Directory.Build.props
mirror .editorconfig editorconfig

for doc in README CONTRIBUTING TESTING REVIEW AGENTS; do
    mirror "$doc.md" "docs/$doc.md"
done
# Stored under another name so Claude Code doesn't load it as a nested CLAUDE.md.
mirror CLAUDE.md docs/CLAUDE.md.template

mirror DotNetExample.slnx projects/DotNetExample.slnx
for project in DotNetExample.*/DotNetExample.*.csproj; do
    mirror "$project" "projects/$(basename "$project")"
done
mirror DotNetExample.Tests/MSTestSettings.cs projects/MSTestSettings.cs.template
if ! diff -q --strip-trailing-cr DotNetExample.Tests/MSTestSettings.cs DotNetExample.IntegrationTests/MSTestSettings.cs > /dev/null; then
    echo "warning: the two MSTestSettings.cs files differ; assets/ ships only the Tests one"
    drift=1
fi

for rule in DotNetExample.Tests/Architecture/*.cs DotNetExample.IntegrationTests/Architecture/*.cs; do
    mirror "$rule" "architecture/$(basename "$rule").template"
done

while IFS= read -r source; do
    case "$source" in
        */Architecture/*|*/MSTestSettings.cs|*Foo*) continue ;;
        DotNetExample.IoC/RegisterDependencies.cs) mirror "$source" "reference/$source.template" without_foo ;;
        *) mirror "$source" "reference/$source.template" ;;
    esac
done < <(git ls-files 'DotNetExample.*/*.cs')

while IFS= read -r existing; do
    relative="${existing#"$assets/"}"
    case "$relative" in
        ci/*) continue ;;
    esac
    if [[ -z "${expected_assets[$relative]:-}" ]]; then
        drift=1
        if $check_only; then
            echo "stale: assets/$relative (no longer in the repo)"
        else
            rm "$existing"
            echo "removed: assets/$relative"
        fi
    fi
done < <(find "$assets" -type f)

if $check_only && [[ $drift -ne 0 ]]; then
    echo "assets/ is out of date. Run scripts/sync-assets.sh, then review SKILL.md (see MAINTAINING.md)."
    exit 1
fi

$check_only && echo "assets/ matches the repo."
exit 0
