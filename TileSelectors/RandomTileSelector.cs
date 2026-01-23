using System;
using System.Collections.Generic;
using CrawfisSoftware.Tiling;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// A tile selector that random picks a tile
    /// </summary>
    public class RandomTileSelector : ITileSelector
    {
        private Random random;

        /// <inheritdoc/>
        public ITile2D DefaultTile { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RandomTileSelector()
        {
            random = new Random();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="random">A random generator to use in the selection</param>
        public RandomTileSelector(Random random)
        {
            this.random = random;
        }

        /// <inheritdoc/>
        public ITile2D GetTile(ITileSet tileSet, int leftChoice, int topChoice, int rightChoice, int bottomChoice, int row = 0, int column = 0)
        {
            // Query the TileSet database for a possible set and then randomly pick one.
            IList<ITile2D> tiles = tileSet.GetMatchingTiles(leftChoice, topChoice, rightChoice, bottomChoice);
            if(tiles.Count == 0)
            {
                return DefaultTile;
            }
            int index = random.Next(tiles.Count);
            return tiles[index];
        }
    }
}
