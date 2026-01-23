using System;
using System.Collections;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    [Flags]
    internal enum BoundaryEdge { Bottom, Top, Left, Right };

    /// <summary>
    /// A tiling enumerator that only returns the tiles within a specified width of the tiling's edges
    /// </summary>
    public class BoundaryTilingEnumerator : ITilingEnumerator
    {
        private int row, column;

        /// <inheritdoc/>
        public int Width { get; set; }

        /// <inheritdoc/>
        public int Height { get; set; }

        /// <summary>
        /// Get or set the width of the boundary to be enumerated
        /// </summary>
        public int BoundaryWidth { get; set; }

        /// <inheritdoc/>
        public Tiling2DIndex Current
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="boundaryWidth">The width of the boundary in the number of tiles</param>
        public BoundaryTilingEnumerator(int boundaryWidth = 1)
        {
            this.BoundaryWidth = boundaryWidth;
            this.Current = new Tiling2DIndex(0, 0);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool cleanup)
        {
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (column >= Width)
            {
                row++;
                column = 0;
            }
            if (row >= Height)
                return false;
            if(row < BoundaryWidth || row > Height-BoundaryWidth-1)
            {
                //column++;
            }
            else if(column == BoundaryWidth)
            {
                column = Width - BoundaryWidth;
            }
            SetCurrent(row*Width+column);
            column++;
            return true;
        }

        // Reset is never called for foreach loops. Seems like best practice is to throw an exception.
        /// <inheritdoc/>
        public void Reset()
        {
            SetCurrent(0);
        }

        private void SetCurrent(int index)
        {
            int i = index % Width;
            int j = index / Width;
            this.Current = new Tiling2DIndex(i, j);
        }
    }
}
