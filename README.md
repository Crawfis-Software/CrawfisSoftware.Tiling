# CrawfisSoftware.Tiling

Core tiling primitives for building Wang-style 2D tilings, maze/dungeon renderers, and SVG/Unity outputs. The library targets `netstandard2.1` and is packaged for both NuGet and Unity UPM. It supplies tiles, tile sets, enumerators, selectors, and builders that higher-level projects consume (see `ExampleProjects`).

## Features
- **Tiles**: `ITile2D`/`Tile2D` expose edge colors and identifiers so adjacent tiles can be constrained.
- **Tile sets**: `ITileSet`, `TileSet`, `CompositeTileSet`, `AbstractMazeTileSet`, `AbstractPartialTileSet`, factories, and a singleton `TileDatabase` for registering and cloning sets.
- **Selection**: `RandomTileSelector` and `SequentialTileSelector` pick tiles that satisfy neighbor constraints with an optional `DefaultTile` fallback.
- **Enumeration**: visit order and masking via `ITilingEnumerator` implementations such as `Sequential`, `Random`, `ZOrder`, `Zigzag`, `Spiral`, `Hilbert`, `Boundary`, `Corner`, `UpperLeftQuadrant`, `Masked`, and `Rectangle` decorator.
- **Builders**: `MatchingTilingBuilder` (constraint-based Wang tilings), `RandomTilingBuilder`, and `TilingBuilder` (stamping/placement utilities) produce concrete `ITiling2D` instances.
- **Utilities**: `TileSetUtilities` for rotation/permutation/remapping, `TilingRemapper`, and explicit tiling containers (`ExplicitTiling2D`).
- **Unity stubs**: `src/UnityStubs` enables non-Unity builds and NuGet consumption.

## Getting started

Install from NuGet:

```bash
dotnet add package CrawfisSoftware.Tiling
```

Unity (UPM) via Git tag:

```
https://github.com/Crawfis-Software/CrawfisSoftware.Tiling.git?path=Packages/com.crawfissoftware.tiling#v<version>
```

## Quick example

```csharp
using CrawfisSoftware.Tiling;
using CrawfisSoftware.Tiling.TileSets;
using CrawfisSoftware.Tiling.TilingBuilders;
using CrawfisSoftware.Tiling.TilingEnumerators;

var tileSet = new AbstractMazeTileSet();
var builder = new MatchingTilingBuilder(width: 8, height: 8)
{
    TilingEnumerator = new SpiralTilingEnumerator(),
    TileSelector = new RandomTileSelector
    {
        DefaultTile = tileSet.DefaultTile
    }
};

builder.UpdateTiling(tileSet);
ITiling2D tiling = builder.GetTiling();
var tile = tiling.Tile(0, 0); // access placed tiles
```

For more end-to-end usage (SVG output, maze generation, Truchet tiles), see `ExampleProjects/TruchetTilingSVG` and `ExampleProjects/MazeTilingSVG`.

## Repository layout
- `src/Common/Runtime`: core interfaces, tile sets, selectors, enumerators, builders, utilities.
- `src/UnityStubs`: minimal types to allow non-Unity builds.
- `Packages/com.crawfissoftware.tiling`: Unity UPM package manifest/assembly definition.
- `ExampleProjects/*`: sample console apps that generate SVG tilings using this library.
- `.github/workflows/nuget-package.yml`: automated packing/publishing workflow.

## Build & publish
- Build/pack NuGet: `dotnet pack CrawfisSoftware.Tiling.csproj -c Release -o ./artifacts` (targets `netstandard2.1`).
- UPM tarball: `npm pack Packages/com.crawfissoftware.tiling` (from repo root) or consume directly via Git URL above.
- Releases: tag `v<version>` to trigger the GitHub Actions workflow that builds, uploads artifacts, and (when configured) pushes to nuget.org via trusted publishing.

## License
CC0 1.0 Universal. See `LICENSE.txt`.
