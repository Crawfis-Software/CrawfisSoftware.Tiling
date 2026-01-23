using System.Collections.Generic;
using CrawfisSoftware.Tiling.TileSets;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// ITileSet provides a container for a set of tiles. This may either be explicit or implicit as in a
    /// selection clauses from a global tile set database (see TileSetDatabaseSlice.cs)
    /// </summary>
    public interface ITileSet : IEnumerable<ITile2D>
    {
        /// <value>
        /// Get the name for the tileset
        /// </value>
        string Name { get; }

        /// <summary>
        /// A short text description of the tileset.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Get the number of tiles in the tileset.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The width of each tile in the tileset.
        /// </summary>
        /// <remarks>This should really be a float.</remarks>
        float Width { get; }

        /// <summary>
        /// The height of each tile in the tileset.
        /// </summary>
        /// <remarks>This should really be a float.</remarks>
        float Height { get; }

        /// <summary>
        /// True if all possible edge color matchings exist in the tileset.
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// An ITile2D to return if GetTile or GetMatchingTiles does not have a valid tile.
        /// </summary>
        ITile2D DefaultTile { get; }

        /// <summary>
        /// A list of strings that describe the tileset.
        /// </summary>
        IEnumerable<string> Keywords { get; }

        /// <summary>
        /// Returns the tile with the specified ID number.
        /// </summary>
        /// <param name="tileID">The tile ID</param>
        /// <returns>Either the tile in tileset with the tileID or the DefaultTile.</returns>
        ITile2D GetTile(int tileID);

        /// <summary>
        /// Returns a list of all tiles matching the specified constraints (or the default tile if none).
        /// </summary>
        /// <param name="leftChoice">A possible edge contraint on the left edge (or no constraint if -1.</param>
        /// <param name="topChoice">A possible edge contraint on the top edge (or no constraint if -1.</param>
        /// <param name="rightChoice">A possible edge contraint on the right edge (or no constraint if -1.</param>
        /// <param name="bottomChoice">A possible edge contraint on the bottom edge (or no constraint if -1.</param>
        /// <returns></returns>
        IList<ITile2D> GetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice);

        /// <summary>
        /// Returns a list of all tiles matching the specified constraints if any exist. Otherwise returns false.
        /// </summary>
        /// <param name="leftChoice">A possible edge contraint on the left edge (or no constraint if -1.</param>
        /// <param name="topChoice">A possible edge contraint on the top edge (or no constraint if -1.</param>
        /// <param name="rightChoice">A possible edge contraint on the right edge (or no constraint if -1.</param>
        /// <param name="bottomChoice">A possible edge contraint on the bottom edge (or no constraint if -1.</param>
        /// <param name="tiles">A list of all tiles matching the specified constraints.</param>
        /// <returns></returns>
        bool TryGetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice, out IList<ITile2D> tiles);
    }
}