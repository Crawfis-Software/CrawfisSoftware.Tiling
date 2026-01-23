using System;
using System.Collections;
using CrawfisSoftware.Tiling;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Simple class to walk through a tiling or 2D grid and emit a Tiling2DIndex of the grid location.
    /// </summary>
    public class SequentialTilingEnumerator : ITilingEnumerator
    {
        private int currentIndex = 0;

        /// <inheritdoc/>
        public int Width { get; set; }
 
        /// <inheritdoc/>
        public int Height { get; set; }

        #region IEnumerator<Tiling2DIndex>
        /// <summary>
        /// The current cell in the enumeration
        /// </summary>
        public Tiling2DIndex Current
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Constructor. Need the Width and Height set.
        ///    Design Note: Not set in the constructor since the tiling is not known.
        /// </summary>
        public SequentialTilingEnumerator()
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
            if (currentIndex >= Width * Height)
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
        #endregion

        private void SetCurrent(int index)
        {
            int i = index % Width;
            int j = index / Width;
            this.Current = new Tiling2DIndex(i, j);
        }
    }
}
