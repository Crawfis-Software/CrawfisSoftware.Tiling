using System;
using System.Collections;
using CrawfisSoftware.Tiling;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Class to walk through a tiling or 2D grid in a spiral from some starting position, emitting a Tiling2DIndex of the grid location.
    /// </summary>
    public class SpiralTilingEnumerator : ITilingEnumerator
    {
        private int _width;
        private int _height;
        private Tiling2DIndex _initialCell;
        int longest;
        int size;
        int cellsEnumerated = 1;
        int x = 0, y = 0;
        int dx = -1;
        int dy = 0;

        /// <summary>
        /// The initial cell to spiral around. Defaults to (0,0). Changing in the middle will result in unknown results.
        /// </summary>
        public Tiling2DIndex InitialCell
        {
            get { return _initialCell; }
            set
            {
                _initialCell = value;
                Reset();
            }
        }

        /// <inheritdoc/>
        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                Reset();
            }
        }

        /// <inheritdoc/>
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                Reset();
            }
        }

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
        public SpiralTilingEnumerator()
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
            return MoveAlongSpiral();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            longest = Math.Max(Width, Height);
            size = longest * longest;
            cellsEnumerated = 0;
            x = 0; 
            y = 0;
            dx = -1;
            dy = 0;
            this.Current = InitialCell;
        }
        #endregion

        private void SetCurrent(int i, int j)
        {
            this.Current = new Tiling2DIndex(i, j);
        }
        private bool MoveAlongSpiral()
        {
            bool foundValidCell = false;
            while(!foundValidCell && cellsEnumerated < Width*Height)
            {
                int cellX = x + InitialCell.i;
                int cellY = y + InitialCell.j;
                // Ensures cell is not out of bounds of tiling size.
                if (0 <= cellX && cellX < Width && 0 <= cellY && cellY < Height)
                {
                    SetCurrent(cellX, cellY);
                    cellsEnumerated++;
                    foundValidCell = true;
                }

                if (x != 0 || y != 0)
                {
                    if ((x == y && x > 0) || (x == y - 1 && x < 0))
                    {
                        dy = -dx;
                        dx = 0;
                    }
                    else if (-x == y)
                    {
                        dx = dy;
                        dy = 0;
                    }
                }

                x = x + dx;
                y = y + dy;
            }
            return foundValidCell;
        }
    }
}
