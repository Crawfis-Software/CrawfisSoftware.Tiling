
using System;
using System.Collections;
using CrawfisSoftware.Tiling;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Sample of a "special" tiling enumerator that only enumerates the
    /// upper-left quadrant.
    /// </summary>
    public class UpperLeftQuadrantEnumerator : ITilingEnumerator
    {
        /// <inheritdoc/>
        public int Width { get; set; }

        /// <inheritdoc/>
        public int Height { get; set; }
        private int currentIndex = 0;

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
        public UpperLeftQuadrantEnumerator()
        {
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
            if (currentIndex >= (Width * Height / 4))
                return false;
            SetCurrent(currentIndex);
            currentIndex++;
            return true;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            currentIndex = 0;
            SetCurrent(currentIndex);
        }

        private void SetCurrent(int index)
        {
            int i = index % (Width/2);
            int j = index / (Width/2);
            this.Current = new Tiling2DIndex(i, j);
        }
    }
}
