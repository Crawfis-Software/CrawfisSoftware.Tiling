using System;
using System.Collections;
using System.Collections.Generic;

using CrawfisSoftware.Tiling;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Enumerate tiles from an explicit set of tile indices only.
    /// </summary>
    public class ExplicitTilingEnumerator : ITilingEnumerator
    {
        /// <inheritdoc/>
        public int Width { get; set; }

        /// <inheritdoc/>
        public int Height { get; set; }

        private int currentIndex = 0;
        private List<Tiling2DIndex> tileLocations = new List<Tiling2DIndex>();

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
        /// <param name="tileLocations">Explicit list of tile locations to enumerate</param>
        public ExplicitTilingEnumerator(IEnumerable<Tiling2DIndex> tileLocations)
        {
            this.Current = new Tiling2DIndex(0, 0);
            foreach (var point in tileLocations)
            {
                this.tileLocations.Add(point);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tileLocations">Explicit list of tile locations to enumerate as cell Id's on a grid.</param>
        /// <param name="width">The width of the underlying grid.</param>
        public ExplicitTilingEnumerator(IEnumerable<int> tileLocations, int width)
        {
            this.Current = new Tiling2DIndex(0, 0);
            foreach (var point in tileLocations)
            {
                this.tileLocations.Add(new Tiling2DIndex(point % width, point / width));
            }
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
            if (currentIndex >= tileLocations.Count)
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
            int i = tileLocations[index].i;
            int j = tileLocations[index].j;
            this.Current = new Tiling2DIndex(i, j);
        }
    }
}