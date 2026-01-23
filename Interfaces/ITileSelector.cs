namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Interface for selecting a tile from a tileSet.
    /// </summary>
    public interface ITileSelector
    {
        /// <summary>
        /// Get or set a tile that is returned on any failed search.
        /// </summary>
        ITile2D DefaultTile { get; set; }

        /// <summary>
        /// Select and return a tile based on edge constraints.
        /// </summary>
        /// <param name="TileSet">The tileSet that is being searched.</param>
        /// <param name="leftChoice">A possible edge constraint on the left edge (or no constraint if -1.</param>
        /// <param name="topChoice">A possible edge constraint on the top edge (or no constraint if -1.</param>
        /// <param name="rightChoice">A possible edge constraint on the right edge (or no constraint if -1.</param>
        /// <param name="bottomChoice">A possible edge constraint on the bottom edge (or no constraint if -1.</param>
        /// <param name="row">The tiling row if applicable. Defaults to zero.</param>
        /// <param name="column">The tiling column if applicable. Defaults to zero.</param>
        /// <returns>An ITile2D</returns>
        ITile2D GetTile(ITileSet TileSet, int leftChoice, int topChoice, int rightChoice, int bottomChoice, int row = 0, int column = 0);
    }
}