# Copilot Instructions

## General Guidelines
- First general instruction
- Second general instruction
- Use precise wording to distinguish filenames (e.g., `package.yml`) from directory paths (e.g., `.github/workflows/`) to avoid confusion.

## GitHub Actions
- The default/release branch for the repository is `master`.
- If GitHub Actions settings are greyed out, it indicates that the user lacks permission to change these settings, which are likely controlled at the organization level.

## Package Management
- The workspace targets .NET Standard 2.1.
- Use NuGet.org Trusted Publishing (OIDC) with an org-level trusted publisher policy for package id glob `CrawfisSoftware.*`.
- An org secret `NUGET_USER` should be set to the user's nuget.org profile name.