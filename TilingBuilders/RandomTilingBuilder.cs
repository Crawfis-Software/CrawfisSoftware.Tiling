using CrawfisSoftware.Tiling.TileSets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Creates a tiling, an ITiling2D, ignoring the edge "colors".
    /// </summary>
    public class RandomTilingBuilder : ITilingBuilder
    {
        /// <summary>
        /// The number of columns in the desired tiling.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The number of rows in the desired tiling.
        /// </summary>
        public int Height { get; }

        private ITile2D[,] tiling;

        /// <summary>
        /// Get or set the order and the set of tile locations in which each tile is visited.
        /// </summary>
        public ITilingEnumerator TilingEnumerator { get; set; } = new SequentialTilingEnumerator();

        /// <summary>
        /// Get or set the algorithm for how a tile is selected for a tile location.
        /// </summary>
        public ITileSelector TileSelector { get; set; } = new RandomTileSelector();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">The number of columns in the desired tiling.</param>
        /// <param name="height">The number of rows in the desired tiling.</param>
        public RandomTilingBuilder(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            tiling = new ITile2D[width,height];
        }

        /// <summary>
        /// Set or optionally replace tiles in the tiling based on the current TilingEnumerator and TileSelector.
        /// </summary>
        /// <param name="tileSet"></param>
        /// <param name="noReplace">A boolean flag that can be used to indicate not to overwrite tiles already placed.</param>
        public void UpdateTiling(ITileSet tileSet, bool noReplace = true)
        {
            TilingEnumerator.Width = Width;
            TilingEnumerator.Height = Height;
            while (TilingEnumerator.MoveNext())
            {
                Tiling2DIndex index = TilingEnumerator.Current;
                ITile2D tile = TileSelector.GetTile(tileSet, -1, -1, -1, -1, index.j, index.i);
                tiling[index.i, index.j] = tile;
            }
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
        /// Create a concrete instance of a tiling (or partial tiling).
        /// </summary>
        public ITiling2D GetTiling()
        {
            return new ExplicitTiling2D(tiling, TileSelector.DefaultTile);
        }
    }
}
