using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// This creates a tileset of all maze and path tiles. Each edge can have a 
    /// color of 0 or 1 (wall or path). It also have all tiles where one or more
    /// edges can be undefined (-1).
    /// </summary>
    public class AbstractPartialTileSet : ITileSet
    {
        const int tileSetID = 1;
        private IList<ITile2D> tileSet = new List<ITile2D>(16);

        /// <inheritdoc/>
        public string Name => "Partial Tiles";

        /// <inheritdoc/>
        public string Description => "Abstract tile set with partial edges set: -1 not set, 0 wall, 1 path";

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
        public ITile2D DefaultTile { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public AbstractPartialTileSet()
        {
            DefaultTile = new Tile2D(this.Name, 0, -1, -1, -1, -1);
            AddTilesToDatabase();
            SelectTileSet();
        }

        private void SelectTileSet()
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

        // Want to ensure at least one edge is -1.
        private void AddTilesToDatabase()
        {
            int uniqueID = 0;
            TileDatabase.Instance.RegisterTileSet(this);
            int[] colors = new int[] { -1, 0, 1 };
            int[] paths = { 0, 1 };
            int bottom = -1;
            foreach (int rightColor in colors)
                foreach (int topColor in colors)
                    foreach (int leftColor in colors)
                    {
                        Tile2D tile = new Tile2D(Name, uniqueID, leftColor, topColor, rightColor, bottom);
                        uniqueID++;
                        TileDatabase.Instance.AddTile(tile);
                    }
            int right = -1;
            foreach (int bottomColor in paths)
                foreach (int topColor in colors)
                    foreach (int leftColor in colors)
                    {
                        if (leftColor + topColor + right + bottomColor == -4)
                            continue;
                        Tile2D tile = new Tile2D(Name, uniqueID, leftColor, topColor, right, bottomColor);
                        uniqueID++;
                        TileDatabase.Instance.AddTile(tile);
                    }
            int top = -1;
            foreach (int bottomColor in paths)
                foreach (int rightColor in paths)
                    foreach (int leftColor in colors)
                    {
                        if (leftColor + top + rightColor + bottomColor == -4)
                            continue;
                        Tile2D tile = new Tile2D(Name, uniqueID, leftColor, top, rightColor, bottomColor);
                        uniqueID++;
                        TileDatabase.Instance.AddTile(tile);
                    }
            int left = -1;
            foreach (int bottomColor in paths)
                foreach (int rightColor in paths)
                    foreach (int topColor in paths)
                    {
                        if (left + topColor + rightColor + bottomColor == -4)
                            continue;
                        Tile2D tile = new Tile2D(Name, uniqueID, left, topColor, rightColor, bottomColor);
                        uniqueID++;
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
