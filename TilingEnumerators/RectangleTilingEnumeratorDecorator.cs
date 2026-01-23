using CrawfisSoftware.Tiling.TilingBuilders;
using System;
using System.Collections;

namespace CrawfisSoftware.Tiling.TilingEnumerators
{
    /// <summary>
    /// Restrict an enumerator to a rectanglular region.
    /// </summary>
    /// <remarks>Note: The width and height of the enumerator are changed!</remarks>
    public class RectangleTilingEnumeratorDecorator : ITilingEnumerator
    {
        private ITilingEnumerator realEnumerator;
        private bool initialized = false;

        /// <summary>
        /// The origin of the rectangle in the underlying tiling
        /// </summary>
        public Tiling2DIndex LowerRight { get; set; }

        /// <summary>
        /// The width and height of the rectangle
        /// </summary>
        public Tiling2DIndex Size { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public RectangleTilingEnumeratorDecorator()
        {
            this.realEnumerator = new SequentialTilingEnumerator();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="realEnumerator">An <c>ITilingEnumerator</c> that this class decorates</param>
        public RectangleTilingEnumeratorDecorator(ITilingEnumerator realEnumerator)
        {
            this.realEnumerator = realEnumerator;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="realEnumerator">An <c>ITilingEnumerator</c> that this class decorates</param>
        /// <param name="origin">Location in the tiling of the rectangle's origin</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        public RectangleTilingEnumeratorDecorator(ITilingEnumerator realEnumerator, Tiling2DIndex origin, int width, int height)
        : this(realEnumerator,origin, new Tiling2DIndex(width,height))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="realEnumerator">An <c>ITilingEnumerator</c> that this class decorates</param>
        /// <param name="origin">Location in the tiling of the rectangle's origin</param>
        /// <param name="size">The width and the height of the rectangle</param>
        public RectangleTilingEnumeratorDecorator(ITilingEnumerator realEnumerator, Tiling2DIndex origin, Tiling2DIndex size)
        {
            this.realEnumerator = realEnumerator;
            this.LowerRight = origin;
            this.Size = size;
        }
        /// <inheritdoc/>
        public int Width
        {
            get { return realEnumerator.Width; }
            set { realEnumerator.Width = value; }
        }
        /// <inheritdoc/>
        public int Height
        {
            get { return realEnumerator.Height; }
            set { realEnumerator.Height = value; }
        }

        /// <inheritdoc/>
        public Tiling2DIndex Current
        {
            get
            {
                if (!initialized)
                    Initialize();
                Tiling2DIndex index = realEnumerator.Current;
                return new Tiling2DIndex(LowerRight.i+index.i,LowerRight.j+index.j);
            }
        }
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
            if (cleanup)
            {
                realEnumerator.Dispose();
                realEnumerator = null;
            }
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            return realEnumerator.MoveNext();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            Initialize();
            realEnumerator.Reset();
        }
        private void Initialize()
        {
            int xMin = LowerRight.i >= Width ? Width - 1 : LowerRight.i;
            xMin = xMin < 0 ? 0 : xMin;
            int width = xMin+Size.i >= Width ? Width - xMin - 1 : Size.i;
            width = width < 0 ? 0 : width;
            int yMin = LowerRight.j >= Height ? Height - 1 : LowerRight.j;
            yMin = yMin < 0 ? 0 : yMin;
            int height = yMin+Size.j >= Height ? Height - yMin - 1 : Size.j;
            height = height < 0 ? 0 : height;
            LowerRight = new Tiling2DIndex(xMin, yMin);
            Size = new Tiling2DIndex(width, height);
            realEnumerator.Width = width;
            realEnumerator.Height = height;
            initialized = true;
        }
    }
}