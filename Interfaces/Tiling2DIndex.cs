namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Struct to hold the integer grid location of a tile.
    /// </summary>
    public struct Tiling2DIndex
    {
        /// <summary>
        /// The i (column) grid location from zero to N-1, where N is the width of the tiling.
        /// </summary>
        public int i;
        /// <summary>
        /// The j (row) grid location from zero to M-1, where M is the height of the tiling.
        /// </summary>
        public int j;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public Tiling2DIndex(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
    }
}