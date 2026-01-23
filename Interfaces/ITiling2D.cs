using System;

[assembly: CLSCompliant(true)]
namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Interface for a tiling.
    /// </summary>
    public interface ITiling2D
    {
        /// <summary>
        /// Get the number of columns in the tiling.
        /// </summary>
        int Width { get; }
        /// <summary>
        /// Get the number of rows in the tiling.
        /// </summary>
        int Height { get; }
        /// <summary>
        /// Given the cell location in a tiling, return the tile at that location.
        /// </summary>
        /// <param name="i">The column location.</param>
        /// <param name="j">The row location.</param>
        /// <returns>The tile stored at the specified location</returns>
        ITile2D Tile(int i, int j);
    }
}