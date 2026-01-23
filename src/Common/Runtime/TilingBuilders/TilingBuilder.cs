using CrawfisSoftware.Tiling.TileSets;
using CrawfisSoftware.Tiling.TilingEnumerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Utility class for creating tilings using stamps or explicit setting of tiles.
    /// </summary>
    public class TilingBuilder : ITilingBuilder
    {
        /// <summary>
        /// The number of columns in the desired tiling.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The number of rows in the desired tiling.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Get or set the order and the set of tile locations in which each tile is visited.
        /// </summary>
        public ITilingEnumerator TilingEnumerator { get; set; } = new SequentialTilingEnumerator();

        /// <summary>
        /// Get or set the algorithm for how a tile is selected for a tile location.
        /// </summary>
        public ITileSelector TileSelector { get; set; } = new RandomTileSelector();

        private ITile2D[,] tiling;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">The number of columns in the desired tiling.</param>
        /// <param name="height">The number of rows in the desired tiling.</param>
        public TilingBuilder(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            tiling = new ITile2D[width, height];
        }

        /// <summary>
        /// Create a concrete instance of a tiling (or partial tiling).
        /// </summary>
        public ITiling2D GetTiling()
        {
            return new ExplicitTiling2D(tiling, TileSelector.DefaultTile);
        }

        /// <summary>
        /// Utility to set an individual tile at the specified location.
        /// </summary>
        /// <param name="i">The column location.</param>
        /// <param name="j">The row location.</param>
        /// <param name="tile">An ITile2D.</param>
        public void SetTile(int i, int j, ITile2D tile)
        {
            tiling[i, j] = tile;
        }

        /// <summary>
        /// Set or optionally replace tiles in the tiling based on the current TilingEnumerator and TileSelector.
        /// </summary>
        /// <param name="tileSet"></param>
        /// <param name="noReplace">A boolean flag that can be used to indicate not to overwrite tiles already placed.</param>
        public void UpdateTiling(ITileSet tileSet, bool noReplace = true)
        {
            // Todo: Make this class Abstract? Not sure why I don't have this. Perhaps this should be a no-op.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Place an existing tiling within this one.
        /// </summary>
        /// <param name="stamp">The tiling to place.</param>
        /// <param name="location">A TilingIndex2D location for the placement origin.</param>
        public void PlaceWithinTiling(ITiling2D stamp, Tiling2DIndex location)
        {
            PlaceWithinTiling(stamp, location, new SequentialTilingEnumerator(), AlwaysSelectStampTile);
        }

        /// <summary>
        /// Place an existing tiling within this one.
        /// </summary>
        /// <param name="stamp">The tiling to place.</param>
        /// <param name="location">A TilingIndex2D location for the placement origin.</param>
        /// <param name="stampEnumerator">An enumerator to control which tiles in the stamp are set (and what order).</param>
        /// <param name="replacementStrategy">A function that takes in a location in the tiling being created, the existing tile
        /// and the new tile from the stamp. It returns the tile that should be used (typically one of these, but could be anything.</param>
        public void PlaceWithinTiling(ITiling2D stamp, Tiling2DIndex location, ITilingEnumerator stampEnumerator, Func<Tiling2DIndex, ITile2D, ITile2D, ITile2D> replacementStrategy)
        {
            var rectEnumerator = new RectangleTilingEnumeratorDecorator(this.TilingEnumerator, location, stamp.Width, stamp.Height);
            var baseTileIndexList = new List<Tiling2DIndex>();
            while (rectEnumerator.MoveNext())
            {
                baseTileIndexList.Add(rectEnumerator.Current);
            }
            var stampTileIndexList = new List<Tiling2DIndex>();
            while(stampEnumerator.MoveNext())
            {
                stampTileIndexList.Add(new Tiling2DIndex(stampEnumerator.Current.i + location.i, stampEnumerator.Current.j + location.j));
            }
            IEnumerable<Tiling2DIndex> tilesToCopy = baseTileIndexList.Intersect(stampTileIndexList);
            SetTiles(stamp, location, tilesToCopy);
        }
        private void SetTiles(ITiling2D stamp, Tiling2DIndex location, IEnumerable<Tiling2DIndex> tileIndices)
        {
            SetTiles(stamp, location, tileIndices, AlwaysSelectStampTile);
        }
        private void SetTiles(ITiling2D stamp, Tiling2DIndex location, IEnumerable<Tiling2DIndex> tileIndices, Func<Tiling2DIndex,ITile2D, ITile2D, ITile2D> replacementStrategy)
        {
            foreach(Tiling2DIndex index in tileIndices)
            {
                ITile2D tile = stamp.Tile(index.i, index.j);
                var stampIndex = new Tiling2DIndex(index.i - location.i, index.j - location.j);
                ITile2D stampTile = stamp.Tile(stampIndex.i, stampIndex.j);
                tiling[index.i, index.j] = replacementStrategy(index, tile, stampTile);
            }
        }
        private static ITile2D AlwaysSelectStampTile(Tiling2DIndex index, ITile2D tile, ITile2D stampTile)
        {
            return stampTile;
        }
    }
}
