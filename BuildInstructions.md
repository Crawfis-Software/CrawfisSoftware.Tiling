# Build & Publish

## UPM (Unity)
- Package path: `Packages/com.crawfissoftware.tiling`.
- Tag for distribution: `git tag v<version>` then `git push origin v<version>`.
- Git consumption URL: `https://github.com/Crawfis-Software/CrawfisSoftware.Tiling.git?path=Packages/com.crawfissoftware.tiling#v<version>` (replace `<version>` with the tag, e.g., `0.1.0`).
- Create a `.tgz`: `npm pack Packages/com.crawfissoftware.tiling` (from repo root, requires Node/npm). Host the resulting `.tgz` or add to a scoped registry.

### Hosting the `.tgz`
- Simple download: attach the `.tgz` to a GitHub Release; consumers can download and add via Unity Package Manager “Add package from tarball…”.
- File server/SharePoint/S3: host the `.tgz` at a stable URL; add via Unity “Add package from tarball…” with that URL.
- Private npm/Scoped registry (e.g., Verdaccio, Azure Artifacts, GitHub Packages npm):
  - Publish the tarball: `npm publish com.crawfissoftware.tiling-<version>.tgz --registry <registry-url>`
  - In Unity’s `Packages/manifest.json`, add `"scopedRegistries"` entry pointing to your registry and scope `"com.crawfissoftware"`, then depend on `"com.crawfissoftware.tiling": "<version>"`.

### Consuming via Git (pin vs latest)
- Recommended (stable): pin to a tag in `Packages/manifest.json`:
  - `"com.crawfissoftware.tiling": "https://github.com/Crawfis-Software/CrawfisSoftware.Tiling.git?path=Packages/com.crawfissoftware.tiling#v<version>"`
- Bleeding edge (not recommended for production): use the default branch head with `#master` (or your default branch). This always pulls the latest commit but can break builds.

### Getting “latest”
- There is no automatic “latest” resolution for Git UPM URLs. Update the tag in your manifest when a new release is published. If you need auto-latest, publish to OpenUPM and let Unity resolve versions via the registry.

### OpenUPM (optional)
- OpenUPM requires adding the package to the community registry. Steps:
  - Install tooling: `npm install -g openupm-cli` (optional for local installs, not for publishing).
  - Fork `https://github.com/openupm/openupm` and add `com.crawfissoftware.tiling` to `data/packages/` per their contribution guide, then open a PR. Once merged, OpenUPM builds from your Git tags automatically.

## NuGet (.NET)
- Build/package: `dotnet pack CrawfisSoftware.Tiling.csproj -c Release -o .\artifacts` (or `dotnet build -c Release` since `GeneratePackageOnBuild` is enabled).
- Trusted Publishing (recommended): configure this repo as a trusted publisher on nuget.org (select branch/tag pattern like `refs/tags/v*`), ensure workflow permissions include `contents: write` and `id-token: write`, then push without a key:
  - `dotnet nuget push .\artifacts\CrawfisSoftware.Tiling.<version>.nupkg --source https://api.nuget.org/v3/index.json`
- API key fallback (if not using trusted publishing): create an API key at `https://www.nuget.org/account/apikeys` and use `--api-key <NUGET_API_KEY>` (store as GitHub secret `NUGET_API_KEY`).
- Target framework: `netstandard2.1` (current). If adding multi-targeting, include `netstandard2.0` for broader Unity support.
- Stubs (`UnityStubs`) are included for non-Unity consumers via `#if !UNITY_5_3_OR_NEWER`.
- Consider adding symbols/source link if desired: set `IncludeSymbols`, `PublishRepositoryUrl`, and `EmbedUntrackedSources` in the csproj.

### Consuming (latest)
- Install latest from nuget.org: `dotnet add package CrawfisSoftware.Tiling` (or add `<PackageReference Include="CrawfisSoftware.Tiling" Version="*" />` in your project file to float to latest). Pin exact versions for reproducibility.

## Versioning
- Bump versions together: `CrawfisSoftware.Tiling.csproj` `<Version>` and `Packages/com.crawfissoftware.tiling/package.json` `version`.
- Tag releases in Git to align with UPM Git consumption.
