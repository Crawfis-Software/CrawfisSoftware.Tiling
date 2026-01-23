using System;
using System.Collections.Generic;
using System.Text;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Concrete tiling (ITiling2D) that takes a double array of tileID's and a 
    /// corresponding tile set (ITileSet)
    /// </summary>
    public class ExplicitTiling2D : ITiling2D
    {
        private ITile2D[,] tiling;
        private ITile2D defaultTile;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tiling">2D array of ITile2D's with [i,j] layout.</param>
        /// <param name="defaultTile">A tile to return in the case of a bad request for a tile from the tiling.</param>
        public ExplicitTiling2D(ITile2D[,] tiling, ITile2D defaultTile = null)
        {
            this.tiling = tiling;
            Width = tiling.GetLength(0);
            Height = tiling.GetLength(1);
            this.defaultTile = defaultTile;
        }

        /// <summary>
        /// The number of cells in the "i"-direction.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The number of cells in the "j"-direction.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Get the tile at the corresponding row (i) and column (j).
        /// </summary>
        /// <param name="i">Index into the "i'-direction (row)</param>
        /// <param name="j">Index into the "i'-direction (row)</param>
        /// <returns></returns>
        public ITile2D Tile(int i, int j)
        {
            ITile2D tile = tiling[i, j];
            if ( tile == null)
            {
                return defaultTile;
            }
            return tile;
        }

        internal void SetTile(int i, int j, ITile2D tile)
        {
            tiling[i, j] = tile;
        }
    }
}
