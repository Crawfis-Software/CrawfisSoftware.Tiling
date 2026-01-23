using CrawfisSoftware.Tiling;
using System;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// Static class for useful methods in the Tiling framework
    /// </summary>
    public static class TileSearchUtility
    {
        /// <summary>
        /// Function delegate that will return true if a tile's edge colors match the input edgeColors.
        /// </summary>
        /// <param name="leftChoice">The color of left edge. A negative value indicates any color will match.</param>
        /// <param name="topChoice">The color of top edge. A negative value indicates any color will match.</param>
        /// <param name="rightChoice">The color of right edge. A negative value indicates any color will match.</param>
        /// <param name="bottomChoice">The color of bottom edge. A negative value indicates any color will match.</param>
        /// <returns>A delegate, bool function(ITile), that takes as input an ITile and returns a boolean. 
        /// Returns true is the tile matches the input colors. Returns false otherwise.</returns>
        /// <example>var list = tileSet.Where<c>&lt;ITile2D&gt;</c>(TileSearchUtility.MeetsConstraints(leftChoice, topChoice, rightChoice, bottomChoice));</example>
        public static Func<ITile2D, bool> MeetsConstraints(int leftChoice, int topChoice, int rightChoice, int bottomChoice)
        {
            // Negative values indicate a freedom of choice, so everything matches that edge.
            return tile => (((leftChoice < 0) || (tile.LeftColor < 0) || tile.LeftColor == leftChoice)) &&
                            (((topChoice < 0) || (tile.TopColor < 0) || tile.TopColor == topChoice)) &&
                            (((rightChoice < 0) || (tile.RightColor < 0) || tile.RightColor == rightChoice)) &&
                            (((bottomChoice < 0) || (tile.BottomColor < 0) || tile.BottomColor == bottomChoice));
        }
    }
}
