namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Interface for a tile with edge colors (constraints)
    /// </summary>
    public interface ITile2D
    {
        /// <summary>
        /// The original tile set this tile instance is from.
        /// </summary>
        string TileSetName { get; }
        /// <summary>
        /// A unique number within a tileset.
        /// </summary>
        int ID { get; }
        /// <summary>
        /// Left edge constraint
        /// </summary>
        int LeftColor { get; }
        /// <summary>
        ///  Right edge constraint.
        /// </summary>
        int RightColor { get; }
        /// <summary>
        /// Top edge constraint.
        /// </summary>
        int TopColor { get; }
        /// <summary>
        /// Bottom edge constraint.
        /// </summary>
        int BottomColor { get; }
    }
}