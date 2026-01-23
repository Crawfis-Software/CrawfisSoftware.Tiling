using System;
using System.Collections.Generic;
using System.Text;
using CrawfisSoftware.Tiling.TileSets;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Creates a tiling, an ITiling2D, where the edge "colors" are matched. Useful for Wang Tilings.
    /// </summary>
    public class MatchingTilingBuilder : ITilingBuilder
    {
        /// <summary>
        /// The number of cells in the "i"-direction.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// The number of cells in the "j"-direction.
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// An enumerator to control the order and possibly subset of tiles to visit.
        /// </summary>
        public ITilingEnumerator TilingEnumerator { get; set; } = new SequentialTilingEnumerator();
        /// <summary>
        /// Instance of an ITileSelector to determine a tile from a possible set of matching tiles.
        /// </summary>
        public ITileSelector TileSelector { get; set; } = new RandomTileSelector();

        private ITile2D[,] tiling;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="width">Number of cells in the "i"-direction.</param>
        /// <param name="height">Number of cells in the "j"-direction.</param>
        public MatchingTilingBuilder(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            tiling = new ITile2D[width, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    tiling[i, j] = null;
                }
            }
        }
        /// <summary>
        /// Constructor that takes in an existing Tiling
        /// </summary>
        /// <param name="width">Number of cells in the "i"-direction.</param>
        /// <param name="height">Number of cells in the "j"-direction.</param>
        /// <param name="existingTiling">An existing tiling used as a starting point.</param>
        public MatchingTilingBuilder(int width, int height, ITiling2D existingTiling)
        {
            this.Width = width;
            this.Height = height;
            tiling = new ITile2D[width, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    tiling[i, j] = existingTiling.Tile(i,j);
                }
            }
        }
        /// <summary>
        /// Create a tiling (or partial tiling) using the current TilingEnumerator and TileSelector. New tiles will come from the passed in tile set.
        /// </summary>
        /// <param name="tileSet">An instance of a ITileSet.</param>
        /// <param name="noReplace">A boolean flag that can be used to indicate not to overwrite tiles already placed.</param>
        public void UpdateTiling(ITileSet tileSet, bool noReplace = true)
        {
            TilingEnumerator.Width = Width;
            TilingEnumerator.Height = Height;
            while (TilingEnumerator.MoveNext())
            {
                int leftConstraint = -1;
                int topConstraint = -1;
                int rightConstraint = -1;
                int bottomConstraint = -1;
                Tiling2DIndex tile = TilingEnumerator.Current;
                int i = tile.i;
                int j =  tile.j;
                if (noReplace && tiling[i, j] != null)
                {
                    continue;
                }
                if ((i > 0) && tiling[i - 1, j] != null)
                    leftConstraint = tiling[i - 1, j].RightColor;
                if ((j < Height - 1) && tiling[i, j + 1] != null)
                    topConstraint = tiling[i, j + 1].BottomColor;
                if ((i < Width-1) && tiling[i + 1, j] != null)
                    rightConstraint = tiling[i + 1, j].LeftColor;
                if ((j > 0) && tiling[i, j - 1] != null)
                    bottomConstraint = tiling[i, j - 1].TopColor;
                tiling[i,j] = TileSelector.GetTile(tileSet, leftConstraint, topConstraint, rightConstraint, bottomConstraint, j, i);
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
