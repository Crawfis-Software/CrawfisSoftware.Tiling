using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawfisSoftware.Tiling.TileSets
{
    public static class TileSetUtilities
    {
        /// <summary>
        /// Remap edge colors using functions
        /// </summary>
        /// <param name="tileSetName">A name for the resulting tileset.</param>
        /// <param name="tileset">The initial tileset.</param>
        /// <param name="leftEdgeRemapper">Function that uses the left edge color to determine a new left edge color.</param>
        /// <param name="topEdgeRemapper">Function that uses the top edge color to determine a new top edge color.</param>
        /// <param name="rightEdgeRemapper">Function that uses the right edge color to determine a new right edge color.</param>
        /// <param name="bottomEdgeRemapper">Function that uses the bottom edge color to determine a new bottom edge color.</param>
        /// <param name="initialID">An initial value for the new tile's unique _id.</param>
        /// <param name="idIncrement">(default of 1) Each _id will be separated by this value.</param>
        /// <returns>A stream of new Tile2D's.</returns>
        public static IEnumerable<ITile2D> RemapEdgeColors(string tileSetName, IEnumerable<ITile2D> tileset, Func<int, int> leftEdgeRemapper, Func<int, int> topEdgeRemapper, Func<int, int> rightEdgeRemapper, Func<int, int> bottomEdgeRemapper, int initialID, int idIncrement = 1)
        {
            int id = initialID;
            foreach (var tile in tileset)
            {
                Tile2D tile2D = new Tile2D(tileSetName, id, leftEdgeRemapper(tile.LeftColor), topEdgeRemapper(tile.TopColor), rightEdgeRemapper(tile.RightColor), bottomEdgeRemapper(tile.BottomColor));
                id += idIncrement;
                yield return tile2D;
            }
        }

        /// <summary>
        /// Remap edge colors using functions
        /// </summary>
        /// <param name="tileSetName">A name for the resulting tileset.</param>
        /// <param name="tileset">The initial tileset.</param>
        /// <param name="edgeRemapper">Function that takes an ITile2D and produces new edge colors as an array of int[4].</param>
        /// <param name="initialID">An initial value for the new tile's unique _id.</param>
        /// <param name="idIncrement">(default of 1) Each _id will be separated by this value.</param>
        /// <returns>A stream of new Tile2D's.</returns>
        public static IEnumerable<ITile2D> RemapEdgeColors(string tileSetName, IEnumerable<ITile2D> tileset, Func<ITile2D, int[]> edgeRemapper, int initialID, int idIncrement = 1)
        {
            int id = initialID;
            foreach (var tile in tileset)
            {
                int[] edgeColors = edgeRemapper(tile);
                Tile2D tile2D = new Tile2D(tileSetName, id, edgeColors[0], edgeColors[1], edgeColors[2], edgeColors[3]);
                id += idIncrement;
                yield return tile2D;
            }
        }

        /// <summary>
        /// Create new tiles with new edge colors "lofted" by the value passed in. 
        /// </summary>
        /// <param name="tileSetName">A name for the resulting tileset.</param>
        /// <param name="tileset">The initial tileset.</param>
        /// <param name="loftValue">Value to be added to each edge color.</param>
        /// <param name="initialID">An initial value for the new tile's unique _id.</param>
        /// <param name="idIncrement">(default of 1) Each _id will be separated by this value.</param>
        /// <returns>A stream of new Tile2D's.</returns>
        public static IEnumerable<ITile2D> LoftColors(string tileSetName, IEnumerable<ITile2D> tileset, int loftValue, int initialID, int idIncrement = 1)
        {
            // Could just call remapper with (int color) => return color + loftValue;
            int id = initialID;
            foreach (var tile in tileset)
            {
                Tile2D tile2D = new Tile2D(tileSetName, id, tile.LeftColor + loftValue, tile.TopColor + loftValue, tile.RightColor + loftValue, tile.BottomColor + loftValue);
                id += idIncrement;
                yield return tile2D;
            }
        }

        public static IEnumerable<ITile2D> RotatedCopies(string tileSetName, IEnumerable<ITile2D> tileset, int initialID, int idIncrement = 1, bool includeSelf = true)
        {
            int id = initialID;
            foreach (var tile in tileset)
            {
                foreach (var rotatedTile in RotatedCopies(tileSetName, tile, id, idIncrement, includeSelf))
                {
                    yield return rotatedTile;
                    id += idIncrement;
                }
            }
        }

        public static IEnumerable<ITile2D> RotatedCopies(string tileSetName, ITile2D tile, int initialID, int idIncrement = 1, bool includeSelf = true)
        {
            Tile2D tile2D;
            int id = initialID;
            if (includeSelf)
            {
                tile2D = new Tile2D(tileSetName, id, tile.LeftColor, tile.TopColor, tile.RightColor, tile.BottomColor);
                yield return tile2D;
                id += idIncrement;
            }
            tile2D = new Tile2D(tileSetName, id, tile.TopColor, tile.RightColor, tile.BottomColor, tile.LeftColor);
            yield return tile2D;
            id += idIncrement;
            tile2D = new Tile2D(tileSetName, id, tile.RightColor, tile.BottomColor, tile.LeftColor, tile.TopColor);
            yield return tile2D;
            id += idIncrement;
            tile2D = new Tile2D(tileSetName, id, tile.BottomColor, tile.LeftColor, tile.TopColor, tile.RightColor);
            yield return tile2D;
        }

        public static ITile2D Rotate90(string tileSetName, ITile2D tile, int initialID)
        {
            Tile2D tile2D = new Tile2D(tileSetName, initialID, tile.BottomColor, tile.LeftColor, tile.TopColor, tile.RightColor);
            return tile2D;
        }

        public static IEnumerable<ITile2D> ConstantTileEdges(string tileSetName, IEnumerable<int> colorList, int initialID, int idIncrement = 1)
        {
            Tile2D tile2D;
            int id = initialID;
            foreach (var color in colorList)
            {
                tile2D = new Tile2D(tileSetName, id, color, color, color, color);
                yield return tile2D;
                id += idIncrement;
            }
        }

        public static IEnumerable<ITile2D> MirroredCopies(string tileSetName, ITile2D tile, int initialID, int idIncrement = 1, bool includeSelf = true)
        {
            Tile2D tile2D;
            int id = initialID;
            if (includeSelf)
            {
                tile2D = new Tile2D(tileSetName, id, tile.LeftColor, tile.TopColor, tile.RightColor, tile.BottomColor);
                yield return tile2D;
                id += idIncrement;
            }
            tile2D = new Tile2D(tileSetName, id, tile.RightColor, tile.TopColor, tile.LeftColor, tile.BottomColor);
            yield return tile2D;
            id += idIncrement;
            tile2D = new Tile2D(tileSetName, id, tile.LeftColor, tile.BottomColor, tile.RightColor, tile.TopColor);
            yield return tile2D;
        }

        public static IEnumerable< ITile2D> AllPermutations(string tileSetName, IEnumerable<int> horizontalColors, IEnumerable<int> verticalColors, int initialID, int idIncrement = 1)
        {
            int id = initialID;
            foreach (int leftEdge in horizontalColors)
            {
                foreach (int topEdge in verticalColors)
                {
                    foreach (int rightEdge in horizontalColors)
                    {
                        foreach (int bottomEdge in verticalColors)
                        {
                            Tile2D tile = new Tile2D(tileSetName, id, leftEdge, topEdge, rightEdge, bottomEdge);
                            yield return tile;
                            id += idIncrement;
                        }
                    }
                }
            }
        }
        public static IEnumerable<ITile2D> AllPermutations(string tileSetName, IEnumerable<int> colors, int initialID, int idIncrement = 1)
        {
            return AllPermutations(tileSetName, colors, colors, initialID, idIncrement);
        }

        /// <summary>
        /// Same as LINQ's Select/Where
        /// </summary>
        /// <param name="tileset">The tileset to filter</param>
        /// <param name="predicate">A predicate function that takes in an iTile2D and determines whether to keep it (output it).</param>
        /// <returns>A stream of desired tiles.</returns>
        public static IEnumerable<ITile2D> Filter(IEnumerable<ITile2D> tileset, Func<ITile2D, bool> predicate)
        {
            foreach(var tile in tileset)
            {
                if(predicate(tile))
                {
                    yield return tile;
                }
            }
        }

        #region Closure - replacing a single color (e.g., -1) with a list of colors
        public static IEnumerable<ITile2D> ExpandColorChoice(string tileSetName, IEnumerable<ITile2D> tileset, int targetColor, IEnumerable<int> colors, int initialID, int idIncrement = 1)
        {
            int id = initialID;
            foreach (var tile in tileset)
            {
                foreach (var tile2D in ExpandColorChoice(tileSetName, tile, targetColor, colors, initialID, idIncrement))
                {
                    yield return tile2D;
                }
            }
        }

        public static IEnumerable<ITile2D> ExpandColorChoice(string tileSetName, ITile2D tile, int targetColor, IEnumerable<int> colors, int initialID, int idIncrement = 1)
        {
            IList<ITile2D> tiles = new List<ITile2D>();
            int id = initialID;
            if (tile.LeftColor == targetColor)
            {
                foreach(var expandedTile in ExpandLeftColorChoice(tileSetName, tile, colors, id, idIncrement))
                    tiles.Add(expandedTile);
            }
            else
            {
                tiles.Add((Tile2D)tile);
            }

            if (tile.TopColor == targetColor)
            {
                var currentTiles = tiles.ToList();
                tiles.Clear();
                id = initialID + currentTiles.Count * idIncrement;
                foreach (var tile2D in currentTiles)
                {
                    foreach(var expandedTile in ExpandTopColorChoice(tileSetName, tile2D, colors, id, idIncrement))
                    {
                        tiles.Add(expandedTile);
                    }
                }
            }

            if (tile.RightColor == targetColor)
            {
                var currentTiles = tiles.ToList();
                tiles.Clear();
                id = initialID + currentTiles.Count * idIncrement;
                foreach (var tile2D in currentTiles)
                {
                    foreach (var expandedTile in ExpandRightColorChoice(tileSetName, tile2D, colors, id, idIncrement))
                    {
                        tiles.Add(expandedTile);
                    }
                }
            }

            if (tile.BottomColor == targetColor)
            {
                var currentTiles = tiles.ToList();
                tiles.Clear();
                id = initialID + currentTiles.Count * idIncrement;
                foreach (var tile2D in currentTiles)
                {
                    foreach (var expandedTile in ExpandBottomColorChoice(tileSetName, tile2D, colors, id, idIncrement))
                    {
                        tiles.Add(expandedTile);
                    }
                }
            }
            return tiles;
        }

        public static IEnumerable<ITile2D> ExpandLeftColorChoice(string tileSetName, ITile2D tile, IEnumerable<int> colors, int initialID, int idIncrement = 1)
        {
            Tile2D tile2D;
            int id = initialID;
            foreach (var color in colors)
            {
                tile2D = new Tile2D(tileSetName, id, color, tile.TopColor, tile.RightColor, tile.BottomColor);
                yield return tile2D;
                id += idIncrement;
            }

        }
        public static IEnumerable<ITile2D> ExpandTopColorChoice(string tileSetName, ITile2D tile, IEnumerable<int> colors, int initialID, int idIncrement = 1)
        {
            Tile2D tile2D;
            int id = initialID;
            foreach (var color in colors)
            {
                tile2D = new Tile2D(tileSetName, id, color, tile.TopColor, tile.RightColor, tile.BottomColor);
                yield return tile2D;
                id += idIncrement;
            }

        }
        public static IEnumerable<ITile2D> ExpandRightColorChoice(string tileSetName, ITile2D tile, IEnumerable<int> colors, int initialID, int idIncrement = 1)
        {
            Tile2D tile2D;
            int id = initialID;
            foreach (var color in colors)
            {
                tile2D = new Tile2D(tileSetName, id, color, tile.TopColor, tile.RightColor, tile.BottomColor);
                yield return tile2D;
                id += idIncrement;
            }

        }
        public static IEnumerable<ITile2D> ExpandBottomColorChoice(string tileSetName, ITile2D tile, IEnumerable<int> colors, int initialID, int idIncrement = 1)
        {
            Tile2D tile2D;
            int id = initialID;
            foreach (var color in colors)
            {
                tile2D = new Tile2D(tileSetName, id, color, tile.TopColor, tile.RightColor, tile.BottomColor);
                yield return tile2D;
                id += idIncrement;
            }

        }
#endregion Closure - replacing a single color (e.g., -1) with a list of colors
    }
}