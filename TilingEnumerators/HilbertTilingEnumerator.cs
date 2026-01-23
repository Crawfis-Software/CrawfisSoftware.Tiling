using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace CrawfisSoftware.Tiling.TilingBuilders
{
    /// <summary>
    /// Enumerate from (0,0) using z-order (also known as Lebesgue or Morton space-filling curve).
    /// </summary>
    public class HilbertTilingEnumerator : ITilingEnumerator
    {
        private int _width;
        private int _height;
        private int _currentIndex = 0;
        private int _maxIndex = -1;
        private int _numBits = 16;
        private int pow2Size;

        /// <summary>
        /// Constructor. Need the Width and Height set.
        ///    Design Note: Not set in the constructor since the tiling is not known.
        /// </summary>
        public HilbertTilingEnumerator()
        {
            this.Current = new Tiling2DIndex(0, 0);
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


        /// <inheritdoc/>
        object IEnumerator.Current => Current;

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
            if (_currentIndex >= _maxIndex)
                return false;
            var cell = HilbertValue(_currentIndex);
            while (cell.Item1 >= Width || cell.Item2 >= Height)
            {
                _currentIndex++;
                if (_currentIndex >= _maxIndex)
                    return false;
                cell = HilbertValue(_currentIndex);
            }
            this.Current = new Tiling2DIndex(cell.Item1, cell.Item2);
            _currentIndex++;
            return true;
        }

        /// <inheritdoc/>
        public void Reset()
        {
            // Adds zero placeholders, this code works up to baseTen = 255
            // Adding more zeros makes the code work for larger numbers, but also slows the algorithm.
            // use 16 --> 65,535, 32 --> 4,294,967,295, and 64 --> 18,446,744,073,709,551,615
            int size = Width * Height;
            int rowLevel = (int)Math.Ceiling(Math.Log(Width, 2));
            int columnLevel = (int)Math.Ceiling(Math.Log(Height, 2));
            int zLevel = Math.Max(rowLevel, columnLevel);
            pow2Size = (int)Math.Pow(2, zLevel);
            _maxIndex = pow2Size * pow2Size;
            _numBits = 2 * zLevel;

            _currentIndex = 0;
            this.Current = new Tiling2DIndex(0, 0);
        }
        #endregion

        private Tuple<int, int> HilbertValue(int i)
        {
            // Convert d to (x,y).
            int rx, ry, s, t = i;
            int x = 0;
            int y = 0;
            for (s = 1; s < pow2Size; s *= 2)
            {
                rx = 1 & (t / 2);
                ry = 1 & (t ^ rx);
                Rotate(s, ref x, ref y, rx, ry);
                x += s * rx;
                y += s * ry;
                t /= 4;
            }
            return new Tuple<int, int>(x, y);
        }

        // Rotates a quadrant as necessary.
        void Rotate(int n, ref int x, ref int y, int rx, int ry)
        {
            if (ry == 0)
            {
                if (rx == 1)
                {
                    x = n - 1 - x;
                    y = n - 1 - y;
                }

                // Swap x and y.
                int t = x;
                x = y;
                y = t;
            }
        }
        private Tuple<int, int> ZValue(int baseTen)
        {
            // Declare necessary variables
            String xString = "", yString = "";
            String baseTwo = Convert.ToString(baseTen, 2);

            while (baseTwo.Length < _numBits)
            {
                baseTwo = "0" + baseTwo;
            }
            for (int i = baseTwo.Length - 1; i >= 0; i--)
            {
                // Splits baseTwo's bits into X and Y.
                if (i % 2 == 0)
                {
                    xString = baseTwo.Substring(i, 1) + xString;
                }
                else
                {
                    yString = baseTwo.Substring(i, 1) + yString;
                }
            }

            // Handles special case where X or Y will be 0.
            if (xString.Equals(""))
            {
                xString = "0";
            }
            if (yString.Equals(""))
            {
                yString = "0";
            }

            // Convert the string to binary.
            int xBin = Int32.Parse(xString);
            int yBin = Int32.Parse(yString);

            // Convert binary to decimal.
            int xDec = BinToDec(xBin);
            int yDec = BinToDec(yBin);
            return new Tuple<int, int>(xDec, yDec);
        }

        // Convert from binary to decimal.
        private int BinToDec(int bin)
        {
            int dec = 0;
            int bitSize = 1;
            while (bin > 0)
            {
                int bit = bin % 10;
                bin /= 10;
                dec += bitSize * bit;
                bitSize *= 2;
            }
            return dec;
        }
    }
}