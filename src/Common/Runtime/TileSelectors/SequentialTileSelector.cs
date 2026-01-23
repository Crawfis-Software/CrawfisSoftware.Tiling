using System;
using System.Collections.Generic;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// An ItileSelector that will select the next tile returned from a set
    /// of matched tiles with the same constraints.
    /// </summary>
    public class SequentialTileSelector : ITileSelector
    {
        private Dictionary<int,int> currentIndices = new Dictionary<int, int>();
        private int maxNumberOfEdgeColors;
        /// <summary>
        /// Get or set the tile to return if no match is found.
        /// </summary>
        public ITile2D DefaultTile { get; set; }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maxNumberOfEdgeColors">Optional. A hint on the maximum number of edges. Used in the
        /// hashing needed for saved searches.</param>
        public SequentialTileSelector(int maxNumberOfEdgeColors = 6)
        {
            this.maxNumberOfEdgeColors = maxNumberOfEdgeColors;
        }

        /// <summary>
        /// Returns a tile based on the constraints specified.
        /// </summary>
        /// <param name="tileSet">The tileset to search.</param>
        /// <param name="leftChoice">A constraint on the left edge or -1 for no constraint.</param>
        /// <param name="topChoice">A constraint on the top edge or -1 for no constraint.</param>
        /// <param name="rightChoice">A constraint on the right edge or -1 for no constraint.</param>
        /// <param name="bottomChoice">A constraint on the bottom edge or -1 for no constraint.</param>
        /// <param name="row">The tiling row if applicable. Defaults to zero.</param>
        /// <param name="column">The tiling column if applicable. Defaults to zero.</param>
        /// <returns>Either the next tile from the match or the default tile.</returns>
        //Bug: This does not do what you might expect, since it is a different set each time if the colors change.
        public ITile2D GetTile(ITileSet tileSet, int leftChoice, int topChoice, int rightChoice, int bottomChoice, int row = 0, int column = 0)
        {
            // Query the TileSet database for a possible set and then pick the next one.
            // I could cache the resulting list of tiles, but the database may change between calls.
            // This would rarely occur as new instances are created for each algorithm.
            // Will do this if performance is important. For now, just cache the index.
            IList<ITile2D> tiles = tileSet.GetMatchingTiles(leftChoice, topChoice, rightChoice, bottomChoice);
            if (tiles.Count == 0)
            {
                return DefaultTile;
            }
            int hashcode = GetHashCodeFromEdgeColors(leftChoice, topChoice, rightChoice, bottomChoice);
            int currentIndex;
            if (!currentIndices.TryGetValue(hashcode, out currentIndex))
            {
                currentIndex = 0;
                currentIndices.Add(hashcode, currentIndex);
            }
            if (currentIndex >= tiles.Count)
                currentIndex = 0;
            ITile2D tile = tiles[currentIndex];
            currentIndex++;
            currentIndices[hashcode] = currentIndex;
            return tile;
        }

        private int GetHashCodeFromEdgeColors(int leftChoice, int topChoice, int rightChoice, int bottomChoice)
        {
            // Compute unique hashcode for Dictionary.
            // Note: Unknown or don't care edges are considered their own set.
            int hashcode = leftChoice + 2; // Avoid the negative one and zero values;
            hashcode = maxNumberOfEdgeColors * hashcode + topChoice + 2;
            hashcode = maxNumberOfEdgeColors * hashcode + rightChoice + 2;
            hashcode = maxNumberOfEdgeColors * hashcode + bottomChoice + 2;
            return hashcode;
        }
    }
}
