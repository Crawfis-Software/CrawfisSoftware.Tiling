using CrawfisSoftware.Tiling.TileSets;
using CrawfisSoftware.Tiling.TilingBuilders;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Static utility class to hold defaults for the enumeration and selection.
    /// </summary>
    public static class TilingUtility
    {
        /// <summary>
        /// Get or set the default tiling enumerator
        /// </summary>
        public static ITilingEnumerator DefaultTilingEnumerator { get; set; }
        /// <summary>
        /// Get or set the default tile selector
        /// </summary>
        public static ITileSelector DefaultTileSelector { get; set; }

        static TilingUtility()
        {
            DefaultTilingEnumerator = new RandomTilingEnumerator();
            DefaultTileSelector = new RandomTileSelector();
        }
    }
}