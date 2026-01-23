using System.Collections.Generic;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Interface for all tiling Enumerators. Inherits IEnumerator<TilingIndex2D></TilingIndex2D>
    /// </summary>
    /// <remarks>Can be used to control the order or tiles and whether a tile is enumerated or not.</remarks>
    public interface ITilingEnumerator : IEnumerator<Tiling2DIndex>
    {
        /// <summary>
        /// The number of columns in the tiling that will be enumerated.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// The number of rows in the tiling that will be enumerated.
        /// </summary>
        int Height { get; set; }
    }
}