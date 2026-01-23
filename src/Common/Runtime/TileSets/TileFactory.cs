using System;
using System.Collections.Generic;
using System.Text;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// Factory to create a ITile2D
    /// </summary>
    /// <seealso cref="TileSetFactory"/>
    public class TileFactory
    {
        private string _tileSetName;
        private int _id;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tileSetName">A name for newly constructed tile set.</param>
        /// <param name="initialID">An initial tile ID (defaults to zero). Subsequent tiles will increment this to give unique tile id's.</param>
        public TileFactory(string tileSetName, int initialID = 0)
        {
            _tileSetName = tileSetName;
            _id = initialID;
        }

        /// <summary>
        /// Create a tile associated with the indicated edge "colors"
        /// </summary>
        /// <param name="left">The "color" associated with the left edge.</param>
        /// <param name="top">The "color" associated with the top edge.</param>
        /// <param name="right">The "color" associated with the right edge.</param>
        /// <param name="bottom">The "color" associated with the bottom edge.</param>
        /// <returns>An ITile2D with a unique tile id associated with the tile set.</returns>
        public ITile2D CreateTile(int left, int top, int right, int bottom)
        {
            var tile = new Tile2D(_tileSetName, _id++, left, top, right, bottom);
            return tile;
        }
    }
}