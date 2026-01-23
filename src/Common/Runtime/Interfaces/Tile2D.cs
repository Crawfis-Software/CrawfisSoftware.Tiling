using System;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Struct for holding information about an individual tile.
    /// </summary>
    public struct Tile2D : ITile2D
    {
        /// <summary>
        /// The original tile set this tile instance is from.
        /// </summary>
        public string TileSetName { get; }

        /// <summary>
        /// A unique number within a tileset.
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// Left edge constraint
        /// </summary>
        public int LeftColor { get; }

        /// <summary>
        ///  Right edge constraint.
        /// </summary>
        public int RightColor { get; }

        /// <summary>
        /// Top edge constraint.
        /// </summary>
        public int TopColor { get; }

        /// <summary>
        /// Bottom edge constraint.
        /// </summary>
        public int BottomColor { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tileSetName">The name of the tileset this tile comes from.</param>
        /// <param name="ID">Unique ID within that tileset.</param>
        /// <param name="left">The left edge color (or unspecified if -1).</param>
        /// <param name="top">The left edge color (or unspecified if -1).</param>
        /// <param name="right">The right edge color (or unspecified if -1).</param>
        /// <param name="bottom">The bottom edge color (or unspecified if -1).</param>
        public Tile2D(string tileSetName, int ID, int left, int top, int right, int bottom)
        {
            TileSetName = tileSetName;
            this.ID = ID;
            LeftColor = left;
            TopColor = top;
            RightColor = right;
            BottomColor = bottom;
        }
    }
}