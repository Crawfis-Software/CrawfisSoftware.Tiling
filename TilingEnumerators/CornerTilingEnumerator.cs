using System;
using System.Collections;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// A tiling enumerator that only returns the 4 corner tiles within a specified width of the tiling's edges
    /// </summary>
    public class CornerTilingEnumerator : ITilingEnumerator
    {
        private static readonly Tiling2DIndex[] corners = new Tiling2DIndex[4] { new Tiling2DIndex(0, 0), new Tiling2DIndex(1, 0), new Tiling2DIndex(0, 1), new Tiling2DIndex(1, 1) };
        private int index = 0;

        /// <inheritdoc/>
        public int Width { get; set; }

        /// <inheritdoc/>
        public int Height { get; set; }

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
        public CornerTilingEnumerator()
        {
            index = 0;
            this.Current = corners[0];
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
            if (index >= 4) return false;
            var tilingIndex = corners[index];
            int row = tilingIndex.j * Height;
            int column = tilingIndex.i * Width;
            SetCurrent(row * Width + column);
            index++;
            return true;
        }

        // Reset is never called for foreach loops. Seems like best practice is to throw an exception.
        /// <inheritdoc/>
        public void Reset()
        {
            SetCurrent(0);
            index = 0;
        }

        private void SetCurrent(int index)
        {
            int i = index % Width;
            int j = index / Width;
            this.Current = new Tiling2DIndex(i, j);
        }
    }
}