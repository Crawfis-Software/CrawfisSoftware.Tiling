using CrawfisSoftware.Tiling;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// An ITileSet that queries the TileDatabase and selects a subset 
    /// of the tiles from the database using a predicate function (or set of 
    /// predicate functions.
    /// </summary>
    public class TileSetSelection : TileSet
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public TileSetSelection(string name, string description, int width, int height)
            : base(name, description, width, height)
        {
        }
        /// <summary>
        /// Use the passed in function: bool predicate(ITile2D tile) to select tiles from
        /// the TileDatabase and add to this tileSet.
        /// </summary>
        /// <param name="predicate"> A predicate function that takes in an ITile2D and returns a bool.</param>
        /// <remarks>Can be called multiple times to build up a tileSet.</remarks>
        /// <example>To select the subset of the abstract maze tiles for a racetrack or a path with no
        /// branches.
        ///    <code>
        ///    var pathTileSet = new TileSetSelection("Path Tiles", "Turns and straights only.", width, height);
        ///    pathTileSet.AddTiles(TurnAndStraightTilesOnly);
        ///    ...
        ///    private bool TurnAndStraightTilesOnly(ITile2D tile) {
        ///    if(tile.Name != "Abstract Maze") return false;
        ///       int sum = tile.LeftColor + tile.TopColor + tile.RightColor + tile.BottomColor;
        ///       if(sum == 2) return true;
        ///       return false;
        ///    }
        ///    </code>
        /// </example>
        public void AddTiles(Func<ITile2D, bool> predicate)
        {
            foreach (var tile in TileDatabase.Instance.FindTiles(predicate))
                m_tileSet.Add(tile);
        }
    }
}
