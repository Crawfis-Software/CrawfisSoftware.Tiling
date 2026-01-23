using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// Provides utility methods for generating, transforming, and manipulating collections of 2D tiles with
    /// customizable edge colors and identifiers.
    /// </summary>
    /// <remarks>The TileSetUtilities class offers a variety of static methods to support common tile set
    /// operations, such as remapping edge colors, generating rotated or mirrored copies, expanding color choices, and
    /// creating all permutations of edge color combinations. These methods are intended to facilitate procedural tile
    /// set generation and transformation scenarios, particularly in applications such as tile-based games or procedural
    /// content generation tools. All methods are thread-safe as they do not maintain internal state.</remarks>
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

        /// <summary>
        /// Generates rotated copies of each tile in the specified tileset, assigning unique IDs to each copy.
        /// </summary>
        /// <remarks>The method iterates through each tile in the provided tileset and yields all rotated
        /// versions, assigning sequential IDs starting from initialID. The original tile is included in the output if
        /// includeSelf is set to true.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="tileset">A collection of tiles to generate rotated copies from.</param>
        /// <param name="initialID">The starting ID to assign to the first generated tile.</param>
        /// <param name="idIncrement">The amount by which to increment the ID for each generated tile. Defaults to 1.</param>
        /// <param name="includeSelf">true to include the original tile in the output; otherwise, false. Defaults to true.</param>
        /// <returns>An enumerable collection of tiles representing all rotated copies, each with a unique ID.</returns>
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

        /// <summary>
        /// Generates a sequence of tiles representing all 90-degree rotations of the specified tile, each with a unique
        /// identifier.
        /// </summary>
        /// <remarks>Each returned tile is assigned a unique identifier, starting from initialID and
        /// incremented by idIncrement for each rotation. The order of the tiles in the sequence corresponds to
        /// successive 90-degree clockwise rotations, starting with the original orientation if includeSelf is
        /// true.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="tile">The tile to be rotated. The colors of its sides are used to create rotated copies.</param>
        /// <param name="initialID">The identifier to assign to the first tile in the sequence.</param>
        /// <param name="idIncrement">The amount by which to increment the identifier for each subsequent tile. Defaults to 1.</param>
        /// <param name="includeSelf">true to include the original, unrotated tile as the first element in the sequence; otherwise, false.</param>
        /// <returns>An enumerable collection of tiles, each representing a 90-degree rotation of the original tile. The
        /// collection contains four tiles if includeSelf is true; otherwise, three.</returns>
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

        /// <summary>
        /// Creates a new tile that represents the input tile rotated 90 degrees clockwise.
        /// </summary>
        /// <remarks>The returned tile has its edge colors rotated such that the bottom edge becomes the
        /// left, the left becomes the top, the top becomes the right, and the right becomes the bottom. The original
        /// tile is not modified.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the rotated tile will belong.</param>
        /// <param name="tile">The tile to rotate 90 degrees clockwise.</param>
        /// <param name="initialID">The identifier to assign to the new rotated tile.</param>
        /// <returns>A new ITile2D instance representing the rotated tile.</returns>
        public static ITile2D Rotate90(string tileSetName, ITile2D tile, int initialID)
        {
            Tile2D tile2D = new Tile2D(tileSetName, initialID, tile.BottomColor, tile.LeftColor, tile.TopColor, tile.RightColor);
            return tile2D;
        }

        /// <summary>
        /// Generates a sequence of tiles with identical edge colors from the specified tile set, assigning consecutive
        /// IDs to each tile.
        /// </summary>
        /// <remarks>Each tile in the returned sequence has all four edges set to the same color value
        /// from the input collection. The tile IDs start at the specified initial value and increase by the specified
        /// increment for each tile.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="colorList">A collection of color values to assign to the edges of each tile. Each value produces one tile with all
        /// edges set to that color.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile.</param>
        /// <param name="idIncrement">The value by which to increment the tile ID for each subsequent tile. Defaults to 1.</param>
        /// <returns>An enumerable collection of tiles, each with all edges set to the corresponding color from the input list
        /// and assigned a unique ID.</returns>
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

        /// <summary>
        /// Generates mirrored copies of a tile with specified identifiers and orientation variations.
        /// </summary>
        /// <remarks>The method creates up to three tiles: the original (if includeSelf is true), a
        /// horizontally mirrored version, and a vertically mirrored version. Each tile is assigned a unique identifier
        /// based on initialID and idIncrement.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="tile">The source tile to mirror. The colors of its sides are used to create mirrored variants.</param>
        /// <param name="initialID">The identifier to assign to the first generated tile.</param>
        /// <param name="idIncrement">The amount by which to increment the identifier for each subsequent tile. Defaults to 1.</param>
        /// <param name="includeSelf">true to include a copy of the original tile as the first result; otherwise, false.</param>
        /// <returns>An enumerable collection of ITile2D instances representing the original tile (if included) and its mirrored
        /// variants. The collection contains two or three tiles, depending on the value of includeSelf.</returns>
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

        /// <summary>
        /// Generates all possible permutations of 2D tiles using the specified edge color sets and assigns unique IDs
        /// to each tile.
        /// </summary>
        /// <remarks>The method produces one tile for every combination of left, top, right, and bottom
        /// edge colors. The tile ID is incremented by the specified value for each tile generated. The order of the
        /// returned tiles reflects the nested iteration over the provided color collections.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="horizontalColors">A collection of integer values representing the possible colors for the left and right edges of each tile.</param>
        /// <param name="verticalColors">A collection of integer values representing the possible colors for the top and bottom edges of each tile.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile.</param>
        /// <param name="idIncrement">The value by which to increment the tile ID for each subsequent tile. Defaults to 1.</param>
        /// <returns>An enumerable collection of ITile2D instances, each representing a unique combination of edge colors and
        /// assigned ID.</returns>
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

        /// <summary>
        /// Generates all possible tile permutations for the specified tile set and color sequence, assigning unique IDs
        /// to each permutation.
        /// </summary>
        /// <param name="tileSetName">The name of the tile set for which permutations are generated. Cannot be null or empty.</param>
        /// <param name="colors">A sequence of color identifiers to use when generating permutations. Cannot be null.</param>
        /// <param name="initialID">The starting ID to assign to the first generated permutation.</param>
        /// <param name="idIncrement">The value by which to increment the ID for each subsequent permutation. Defaults to 1.</param>
        /// <returns>An enumerable collection of ITile2D objects representing all generated tile permutations. The collection may
        /// be empty if no permutations can be generated.</returns>
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
        /// <summary>
        /// Expands each tile in the specified tileset by replacing the target color with each color in the provided
        /// collection, generating a new set of tiles for each color substitution.
        /// </summary>
        /// <remarks>This method is useful for generating tile variants with different color options, such
        /// as for palette swaps or theme variations. The method preserves the order of the original tiles and assigns
        /// unique identifiers to each generated tile based on the initial ID and increment.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the tiles belong. Used to identify the context for color expansion.</param>
        /// <param name="tileset">The collection of tiles to process. Each tile in this collection will be expanded by replacing the target
        /// color.</param>
        /// <param name="targetColor">The color value within each tile to be replaced. All occurrences of this color in a tile will be
        /// substituted.</param>
        /// <param name="colors">A collection of color values to substitute for the target color. Each color in this collection will result
        /// in a new tile variant.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile. Each subsequent tile will have its ID
        /// incremented by the specified value.</param>
        /// <param name="idIncrement">The value by which to increment the tile identifier for each generated tile. Defaults to 1.</param>
        /// <returns>An enumerable collection of tiles, where each original tile is expanded into multiple tiles with the target
        /// color replaced by each color in the provided collection.</returns>
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

        /// <summary>
        /// Expands the specified tile by replacing any of its sides that match the target color with all possible color
        /// choices from the provided set, generating new tile variations as needed.
        /// </summary>
        /// <remarks>The method expands each side of the tile that matches the target color independently,
        /// generating all combinations where applicable. The resulting tiles are assigned unique IDs starting from the
        /// specified initial ID, incremented by the given value. The order of expansion is left, top, right, then
        /// bottom.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the tile belongs. Used to identify the context for expansion.</param>
        /// <param name="tile">The tile to expand. Sides of this tile that match the target color will be replaced with each color from the
        /// provided set.</param>
        /// <param name="targetColor">The color value to match on the tile's sides. Any side of the tile with this color will be expanded.</param>
        /// <param name="colors">A collection of color values to use when expanding sides that match the target color. Each value will be
        /// substituted in place of the target color.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile. Subsequent tiles will have incremented IDs.</param>
        /// <param name="idIncrement">The amount by which to increment the tile ID for each generated tile. Defaults to 1.</param>
        /// <returns>An enumerable collection of tiles representing all possible expansions of the original tile, with each side
        /// that matched the target color replaced by every color in the provided set. If no sides match the target
        /// color, the original tile is returned.</returns>
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

        /// <summary>
        /// Generates a sequence of tiles by varying the left color for each specified color value, assigning unique IDs
        /// to each tile.
        /// </summary>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="tile">The base tile whose top, right, and bottom colors are used for each generated tile.</param>
        /// <param name="colors">A collection of color values to assign to the left side of each generated tile.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile.</param>
        /// <param name="idIncrement">The amount by which to increment the tile ID for each subsequent tile. Defaults to 1.</param>
        /// <returns>An enumerable collection of tiles, each with a unique ID and a left color from the specified collection. The
        /// sequence contains one tile for each color value provided.</returns>
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

        /// <summary>
        /// Generates a sequence of tiles by varying the left color for each specified color value, while preserving the
        /// other color properties of the given tile.
        /// </summary>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="tile">The base tile whose top, right, and bottom color values are used for each generated tile.</param>
        /// <param name="colors">A collection of color values to assign to the left side of each generated tile.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile.</param>
        /// <param name="idIncrement">The amount by which to increment the identifier for each subsequent tile. Defaults to 1.</param>
        /// <returns>An enumerable collection of tiles, each with a unique identifier and a left color from the specified
        /// collection. The other color properties are copied from the base tile.</returns>
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

        /// <summary>
        /// Generates a sequence of tiles by creating a new tile for each specified color, incrementing the tile ID for
        /// each generated tile.
        /// </summary>
        /// <remarks>The generated tiles will have their left color set from the provided colors, while
        /// the top, right, and bottom colors are copied from the base tile. The tile ID for each generated tile starts
        /// at the specified initial ID and increases by the specified increment.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="tile">The base tile whose top, right, and bottom colors are used for each generated tile.</param>
        /// <param name="colors">A collection of color values to assign to the left side of each generated tile.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile.</param>
        /// <param name="idIncrement">The value by which to increment the tile ID for each subsequent tile. Defaults to 1.</param>
        /// <returns>An enumerable collection of tiles, each with a unique ID and a left color from the specified collection. The
        /// sequence contains one tile for each color provided.</returns>
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

        /// <summary>
        /// Generates a sequence of tiles by varying the left color for each specified color value, while preserving the
        /// other color properties of the given tile.
        /// </summary>
        /// <remarks>The method yields tiles in the order of the provided color values. The identifier for
        /// each tile starts at the specified initial value and increases by the given increment for each
        /// tile.</remarks>
        /// <param name="tileSetName">The name of the tile set to which the generated tiles will belong.</param>
        /// <param name="tile">The base tile whose top, right, and bottom color values are used for each generated tile.</param>
        /// <param name="colors">A collection of integer color values to assign to the left side of each generated tile.</param>
        /// <param name="initialID">The starting identifier to assign to the first generated tile.</param>
        /// <param name="idIncrement">The value by which to increment the tile identifier for each subsequent tile. The default is 1.</param>
        /// <returns>An enumerable collection of tiles, each with a unique identifier and a left color from the specified
        /// collection. The sequence contains one tile for each color value provided.</returns>
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