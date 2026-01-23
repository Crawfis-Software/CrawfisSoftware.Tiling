using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// A tile set (abstract) that has the 16 possible configurations of walls and doors
    /// </summary>
    public class AbstractMazeTileSet : ITileSet
    {
        const int tileSetID = 0;
        private IList<ITile2D> tileSet = new List<ITile2D>(16);

        /// <inheritdoc/>
        public string Name => "Abstract Maze";

        /// <inheritdoc/>
        public string Description => "Abstract graph tile set for maze, dungeon, etc.";

        /// <inheritdoc/>
        public float Width { get; } = -1;

        /// <inheritdoc/>
        public float Height { get; } = -1;

        /// <inheritdoc/>
        public int Count { get; } = 16;

        /// <inheritdoc/>
        public bool IsComplete { get; } = true;

        /// <inheritdoc/>
        public IEnumerable<String> Keywords { get; } = new string[] { "maze", "path", "dungeon", "graph", "abstract" };

        /// <inheritdoc/>
        public ITile2D DefaultTile { get; private set; } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public AbstractMazeTileSet()
        {
            AddTilesToDatabase();
            BuildTileSet();
        }

        private void BuildTileSet()
        {
            foreach (var tile in TileDatabase.Instance.Tiles)
            {
                if (tile.TileSetName == Name)
                    tileSet.Add(tile);
            }
        }


        /// <inheritdoc/>
        public ITile2D GetTile(int tileID)
        {
            ITile2D tile = TileDatabase.Instance.GetTile(Name, tileID);
            return tile;
        }

        /// <inheritdoc/>
        public IList<ITile2D> GetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice)
        {
            var list = tileSet.Where<ITile2D>(TileSearchUtility.MeetsConstraints(leftChoice, topChoice, rightChoice, bottomChoice));
            return list.ToList<ITile2D>();
        }
        /// <inheritdoc/>
        public bool TryGetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice, out IList<ITile2D> tiles)
        {
            var list = tileSet.Where<ITile2D>(TileSearchUtility.MeetsConstraints(leftChoice, topChoice, rightChoice, bottomChoice));
            tiles = list.ToList<ITile2D>();
            if (tiles.Count > 0)
                return true;
            return false;
        }

        private void AddTilesToDatabase()
        {
            int uniqueID = 0;
            TileDatabase.Instance.RegisterTileSet(this);
            int[] colors = new int[] { 0, 1 };
            foreach (int bottomColor in colors)
                foreach (int rightColor in colors)
                    foreach (int topColor in colors)
                        foreach (int leftColor in colors)
                        {
                            uniqueID = leftColor << 3;
                            uniqueID += topColor << 2;
                            uniqueID += rightColor << 1;
                            uniqueID += bottomColor;
                            Tile2D tile = new Tile2D(Name, uniqueID, leftColor, topColor, rightColor, bottomColor);
                            //uniqueID++;
                            TileDatabase.Instance.AddTile(tile);
                        }
        }

        /// <inheritdoc/>
        public IEnumerator<ITile2D> GetEnumerator()
        {
            return tileSet.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
