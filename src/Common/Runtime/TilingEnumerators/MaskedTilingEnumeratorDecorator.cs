using System;
using System.Collections;

namespace CrawfisSoftware.Tiling.TilingEnumerators
{
    /// <summary>ITilingEnumerator decorator that uses a predicate function to determine
    /// whether to yield each tile index or not.
    /// </summary>
    public class MaskedTilingEnumeratorDecorator : ITilingEnumerator
    {
        private ITilingEnumerator realEnumerator;
        private Predicate<Tiling2DIndex> maskFunction;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="realEnumerator">A ITilingEnumerator to decorate.</param>
        /// <param name="maskFunction">A function that takes a Tiling2DIndex and returns true or false. If true, this
        /// index is enumerated.</param>
        public MaskedTilingEnumeratorDecorator(ITilingEnumerator realEnumerator, Predicate<Tiling2DIndex> maskFunction)
        {
            this.realEnumerator = realEnumerator;
            this.maskFunction = maskFunction;
        }

        /// <inheritdoc/>
        public int Width
        {
            get { return realEnumerator.Width; }
            set { realEnumerator.Width = value; }
        }

        /// <inheritdoc/>
        public int Height {
            get { return realEnumerator.Height; }
            set { realEnumerator.Height = value; }
        }

        /// <inheritdoc/>
        public Tiling2DIndex Current => realEnumerator.Current;

        /// <inheritdoc/>
        object IEnumerator.Current => this.Current;

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool cleanup)
        {
            realEnumerator = null;
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            bool canMove = realEnumerator.MoveNext();
            while ( canMove && !maskFunction(realEnumerator.Current)) {
                canMove = realEnumerator.MoveNext();
            }
            return canMove;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            realEnumerator.Reset();
        }
    }
}