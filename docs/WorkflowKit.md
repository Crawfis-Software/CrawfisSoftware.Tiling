# Workflow kit (NuGet + Release assets + optional Unity UPM)

The workflow kit will:

- build + pack a .NET library into NuGet artifacts (`.nupkg` + `.snupkg`)
- optionally create a Unity Package Manager (UPM) tarball (`.tgz`) if a Unity package folder exists
- publish NuGet packages to nuget.org using OIDC (`NuGet/login@v1`)
- attach artifacts to the GitHub Release

This repo contains a reusable GitHub Actions setup that you can reuse in two ways:
## **Preferred: use the zip kit**
   - Unzip the workflow-kit.zip file into another repo root and adjust a few `env` values (see below).

## **Manual: copy individual files**
Copy these files and folders into the target repo root:

- `.github/workflows/nuget-package.yml`
- `.github/workflows/release-please.yml`
- `.github/workflows/commitlint.yml`
- `.release-please-config.json`
- `.release-please-manifest.json`
- `commitlint.config.cjs`
- `docs/WorkflowKit.md`

## Minimal repo-specific edits

Edit `.github/workflows/nuget-package.yml` and update the top-level `env` values:

- `DOTNET_PROJECT`: path to the `.csproj` to pack
- `UPM_PACKAGE_ID`: e.g. `com.yourcompany.yourlib`
- `UPM_PACKAGE_DIR`: e.g. `Packages/com.yourcompany.yourlib`

If a repo has **no Unity package**, you can leave the UPM values alone; the workflow will just print a skip message.

## Required GitHub settings

### Secrets

- `NUGET_USER`: username/email used by `NuGet/login@v1` to mint a temporary API key via OIDC

### Environment

- Create an environment named `release` (used by the `publish-nuget` job)

## Expected repo structure (recommended)

- `<RepoRoot>/<YourLibrary>.csproj`
- Optional Unity UPM folder: `Packages/<UPM_PACKAGE_ID>/package.json`

## Creating a new “drop-in zip”

If you change the workflow and want to create a new zip, there is a PowerShell script for that.
From the repository root in a PowerShell-capable terminal (including the GitHub Copilot terminal in Visual Studio), run:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tools/create-workflow-kit.ps1 -OutFile workflow-kit.zip
```

If you omit `-OutFile`, the script will default to `workflow-kit.zip` in the repo root:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File tools/create-workflow-kit.ps1
```

The script:

- validates that all expected files exist
- stages them with their relative paths (e.g., `.github/workflows/...`, `docs/WorkflowKit.md`)
- creates `workflow-kit.zip` in the repo root

## Using the kit in another repository

1. Unzip `workflow-kit.zip` into the target repo root.
2. Edit the `env` section at the top of `.github/workflows/nuget-package.yml` (`DOTNET_PROJECT`, `UPM_PACKAGE_ID`, `UPM_PACKAGE_DIR`).
3. Add the `NUGET_USER` secret and ensure the `release` environment exists.
4. Push a tag `v<x.y.z>` or publish a release to exercise the workflow end to end
