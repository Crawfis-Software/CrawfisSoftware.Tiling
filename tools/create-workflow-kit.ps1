param(
  [string]$OutFile = "workflow-kit.zip"
)

$ErrorActionPreference = 'Stop'

$repoRoot = (Resolve-Path '.').Path

$items = @(
  '.github/workflows/nuget-package.yml',
  '.github/workflows/release-please.yml',
  '.github/workflows/commitlint.yml',
  '.release-please-config.json',
  '.release-please-manifest.json',
  'commitlint.config.cjs',
  'docs/WorkflowKit.md'
)

$missing = $items | Where-Object { -not (Test-Path $_) }
if ($missing.Count -gt 0) {
  throw "Missing required files: $($missing -join ', ')"
}

if (Test-Path $OutFile) {
  Remove-Item $OutFile -Force
}

$stagingDir = Join-Path $repoRoot '.workflow-kit-staging'
if (Test-Path $stagingDir) {
  Remove-Item $stagingDir -Recurse -Force
}

New-Item -ItemType Directory -Path $stagingDir | Out-Null

foreach ($item in $items) {
  $source = Join-Path $repoRoot $item
  $destination = Join-Path $stagingDir $item
  $destinationDir = Split-Path $destination -Parent
  New-Item -ItemType Directory -Path $destinationDir -Force | Out-Null
  Copy-Item -Path $source -Destination $destination -Force
}

Compress-Archive -Path (Join-Path $stagingDir '*') -DestinationPath $OutFile -Force
Remove-Item $stagingDir -Recurse -Force

Write-Host "Created $OutFile"
