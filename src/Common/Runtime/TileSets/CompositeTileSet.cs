using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// A collection of tileSets that act as a single tileSet
    /// </summary>
    public class CompositeTileSet : ITileSet
    {
        private List<ITileSet> tileSets = new List<ITileSet>();

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string Description { get; set; }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                int count = 0;
                foreach (var tileSet in tileSets)
                    count += tileSet.Count;
                return count;
            }
        }

        /// <inheritdoc/>
        public float Width { get; set; }

        /// <inheritdoc/>
        public float Height { get; set; }

        /// <inheritdoc/>
        public bool IsComplete { get; set; }

        /// <inheritdoc/>
        public IEnumerable<string> Keywords { get; set; }

        /// <inheritdoc/>
        public ITile2D DefaultTile { get; set; }

        /// <summary>
        /// Iterate over all of the tileSets in this class
        /// </summary>
        public IEnumerable<ITileSet> TileSets
        {
            get
            {
                return tileSets;
            }
        }

        /// <inheritdoc/>
        public ITile2D GetTile(int tileID)
        {
            if (tileID == DefaultTile.ID)
                return DefaultTile;
            foreach (var tileSet in tileSets)
            {
                ITile2D tile = tileSet.GetTile(tileID);
                if (tile.ID != tileSet.DefaultTile.ID)
                    return tile;
            }
            return DefaultTile;
        }

        /// <inheritdoc/>
        public IList<ITile2D> GetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice)
        {
            List<ITile2D> list = new List<ITile2D>();
            foreach (var tileSet in tileSets)
            {
                foreach (var tile in tileSet.Where<ITile2D>(TileSearchUtility.MeetsConstraints(leftChoice, topChoice, rightChoice, bottomChoice)))
                {
                    list.Add(tile);
                }
            }
            return list;
        }

        /// <summary>
        /// Add a tile set to the composite list of tileSets
        /// </summary>
        /// <param name="tileSet"></param>
        public void AddTileSet(ITileSet tileSet)
        {
            tileSets.Add(tileSet);
        }

        /// <inheritdoc/>
        public bool TryGetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice, out IList<ITile2D> tiles)
        {
            tiles = GetMatchingTiles(leftChoice, topChoice, rightChoice, bottomChoice);
            if (tiles == null || tiles.Count == 0) return false;
            return true;
        }

        /// <inheritdoc/>
        public IEnumerator<ITile2D> GetEnumerator()
        {
            foreach (var tileSet in tileSets)
                foreach (var tile in tileSet)
                    yield return tile;
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
