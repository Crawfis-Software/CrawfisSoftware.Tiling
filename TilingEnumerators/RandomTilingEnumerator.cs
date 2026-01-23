using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Iterate over all of the tiles in a tiling randomly.
    /// </summary>
    public class RandomTilingEnumerator : ITilingEnumerator
    {
        /// <inheritdoc/>
        public int Width { get; set; }

        /// <inheritdoc/>
        public int Height { get; set; }

        private Random random;
        private int[] permutation;
        private int currentIndex = 0;
        private bool initialized = false;
        private Tiling2DIndex current;

        /// <inheritdoc/>
        public Tiling2DIndex Current
        {
            get
            {
                if (!initialized)
                    Initialize();
                return current;
            }
            private set
            {
                current = value;
            }
        }

        /// <inheritdoc/>
        object IEnumerator.Current => Current;

        /// <summary>
        /// Constructor
        /// </summary>
        public RandomTilingEnumerator()
            : this(new Random())
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="random">A random number generator</param>
        public RandomTilingEnumerator(Random random)
        {
            this.random = random;
        }

        private void Initialize()
        {
            CreateRandomPermutation();
            SetCurrent(0);
            initialized = true;
        }

        // This is the "inside-out" algorithm of Fisher and Yates' shuffle algorithm (Knuth Algorithm P)
        // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        private void CreateRandomPermutation()
        {
            int numberOfTiles = Width * Height;
            permutation = new int[numberOfTiles];
            for (int i = 0; i < numberOfTiles; i++)
            {
                int index = random.Next(i + 1);
                if (index != i)
                    permutation[i] = permutation[index];
                permutation[index] = i;
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
            if(!initialized)
                Initialize();
            if (currentIndex >= Width * Height)
                return false;
            SetCurrent(currentIndex);
            currentIndex++;
            return true;
        }

        // Reset is never called for foreach loops. Seems like best practice is to throw an exception.
        /// <inheritdoc/>
        public void Reset()
        {
            initialized = false;
            currentIndex = 0;
            SetCurrent(currentIndex);
        }

        private void SetCurrent(int index)
        {
            int i = permutation[index] % Width;
            int j = permutation[index] / Width;
            this.Current = new Tiling2DIndex(i, j);
        }
    }
}
