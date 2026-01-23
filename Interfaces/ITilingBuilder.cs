namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Interface for creating tilings.
    /// </summary>
    public interface ITilingBuilder
    {
        /// <summary>
        /// Get or set the order and the set of tile locations in which each tile is visited.
        /// </summary>
        ITilingEnumerator TilingEnumerator { get; set; }
        /// <summary>
        /// Get or set the algorithm for how a tile is selected for a tile location.
        /// </summary>
        ITileSelector TileSelector { get; set; }
        /// <summary>
        /// Set or optionally replace tiles in the tiling based on the current TilingEnumerator and TileSelector.
        /// </summary>
        /// <param name="tileSet"></param>
        /// <param name="noReplace">A boolean flag that can be used to indicate not to overwrite tiles already placed.</param>
        void UpdateTiling(ITileSet tileSet, bool noReplace = true);

        /// <summary>
        /// Utility to set an individual tile at the specified location.
        /// </summary>
        /// <param name="i">The column location.</param>
        /// <param name="j">The row location.</param>
        /// <param name="tile">An ITile2D.</param>
        void SetTile(int i, int j, ITile2D tile);

        /// <summary>
        /// Create a concrete instance of a tiling (or partial tiling).
        /// </summary>
        ITiling2D GetTiling();
    }
}
