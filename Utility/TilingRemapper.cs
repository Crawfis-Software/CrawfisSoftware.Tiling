using CrawfisSoftware.Tiling.TileSets;
using CrawfisSoftware.Tiling.TilingBuilders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Class to configure and remap tiles in a tiling. Can be used to map from an abstract
    /// tiling to a concrete, replace tiles on a path, etc.
    /// </summary>
    public class TilingRemapper
    {
        static ITilingEnumerator defaultEnumerator = new SequentialTilingEnumerator();
        private ITiling2D originalTiling;
        private ITileSet newTileSet;
        private Random random;
        private IDictionary<int, List<int>> conformantTiles = new Dictionary<int, List<int>>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalTiling">The underlying tiling</param>
        /// <param name="newTileSet">A tileset to use to replace existing tiles</param>
        public TilingRemapper(ITiling2D originalTiling, ITileSet newTileSet)
            : this(originalTiling, newTileSet, new Random())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="originalTiling">The underlying tiling</param>
        /// <param name="newTileSet">A tileset to use to replace existing tiles</param>
        /// <param name="random">A random generator used when multiple tiles can be used
        /// in the remapping</param>
        public TilingRemapper(ITiling2D originalTiling, ITileSet newTileSet, Random random)
        {
            this.originalTiling = originalTiling;
            this.newTileSet = newTileSet;
            this.random = random;
        }
        
        /// <summary>
        /// Create a new tiling, remapping tiles where indicated
        /// </summary>
        /// <param name="tilingEnumerator">The set of tiles to consider in the remapping</param>
        /// <returns>A new tiling that has some or all of the tiles replaced from the original tiling</returns>
        public ITiling2D Remap(ITilingEnumerator tilingEnumerator)
        {
            tilingEnumerator.Width = originalTiling.Width;
            tilingEnumerator.Height = originalTiling.Height;
            ITile2D[,] newTiling = new ITile2D[originalTiling.Width, originalTiling.Height];
            for (int j = 0; j < originalTiling.Height; j++)
            {
                for (int i = 0; i < originalTiling.Width; i++)
                {
                    newTiling[i, j] = originalTiling.Tile(i, j);
                }
            }
            while(tilingEnumerator.MoveNext())
            {
                Tiling2DIndex tileLocation = tilingEnumerator.Current;
                ITile2D tile = originalTiling.Tile(tileLocation.i, tileLocation.j);
                var tileList = conformantTiles[tile.ID];
                // TODO: pass in a selector for the list. But that interfaces with a tileset, not a list
                int newTile = tileList[random.Next(tileList.Count)];
                newTiling[tileLocation.i, tileLocation.j] = newTileSet.GetTile(newTile);
            }
            return new ExplicitTiling2D(newTiling);
        }

        /// <summary>
        /// Add a mapping between tileID's
        /// </summary>
        /// <param name="originalTile">The tile ID to be replaced</param>
        /// <param name="newPossibleTile">A tile ID to be included in the set of possible replacements</param>
        public void AddMapping(int originalTile, int newPossibleTile)
        {
            if (!conformantTiles.Keys.Contains(originalTile))
                conformantTiles[originalTile] = new List<int>();
            conformantTiles[originalTile].Add(newPossibleTile);
        }
    }
}
