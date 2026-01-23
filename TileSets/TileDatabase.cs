using CrawfisSoftware.Tiling.TileSets;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Singleton class that can be used as a database of tiles or tilesets
    /// </summary>
    public class TileDatabase
    {
        private List<ITile2D> tiles = new List<ITile2D>();
        private List<TileSetInfo> tileSets = new List<TileSetInfo>();

        /// <summary>
        /// Get the instance of the tile database
        /// </summary>
        public static TileDatabase Instance { get; private set; }
 
        /// <summary>
        /// Get the total number of tiles in the database
        /// </summary>
        public int NumberOfTiles { get { return tiles.Count; } }

        /// <summary>
        /// Get the number of distinct tilesets in the database
        /// </summary>
        public int NumberOfTileSets { get { return tileSets.Count; } }

        /// <summary>
        /// Enumerate through all of the tiles in the database
        /// </summary>
        public IEnumerable<ITile2D> Tiles
        {
            get { return tiles; }
        }

        static TileDatabase()
        {
            Instance = new TileDatabase();
        }

        private TileDatabase()
        {

        }

        /// <summary>
        /// This will register a new tileset with the database. No tiles will be added.
        /// </summary>
        /// <param name="tileSet">The ITileSet to register</param>
        public void RegisterTileSet(ITileSet tileSet)
        {
            TileSetInfo info = new TileSetInfo(tileSet.Name, tileSet.Description, tileSet.Count, tileSet.Width, tileSet.Height, tileSet.IsComplete, tileSet.Keywords);
            tileSets.Add(info);
        }

        /// <summary>
        /// Get the specified tile based on the tileset name and tileID
        /// </summary>
        /// <param name="tileSetName">The name of the tileset</param>
        /// <param name="tileID">Unique tile ID within this tileset</param>
        /// <returns>The requested tile</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the tile does not exist in the database</exception>
        public ITile2D GetTile(string tileSetName, int tileID)
        {
            foreach(var tile in tiles)
            {
                if (tile.TileSetName == tileSetName && tile.ID == tileID)
                    return tile;
            }
            throw new ArgumentOutOfRangeException("No tile exists in the database matching the tileSetID and tileID");
        }

        /// <summary>
        /// Add a tile to the database
        /// </summary>
        /// <param name="tile">The tile to add</param>
        public void AddTile(ITile2D tile)
        {
            tiles.Add(tile);
        }

        private IEnumerable<ITile2D> GetTileSetTiles(string tileSetName)
        {
            foreach (var tile in tiles)
            {
                if (tile.TileSetName == tileSetName)
                    yield return tile;
            }
        }

       internal TileSetInfo GetTileSetInfo(string tileSetName)
        {
            foreach (var tileSet in tileSets)
            {
                if (tileSet.Name == tileSetName)
                {
                    return tileSet;
                }
            }
            return TileSetInfo.Null;
        }

        /// <summary>
        /// Returns a new instance of the tileset
        /// </summary>
        /// <param name="tileSetName">The tile set name</param>
        /// <returns>A new instance of the tileset</returns>
        /// <see cref="FindTiles"/>
        public ITileSet GetTileSet(string tileSetName)
        {
            TileSetInfo tileSetInfo = GetTileSetInfo(tileSetName);
            if (tileSetInfo.Name != tileSetName)
                return null;
            TileSet tileSet = new TileSet(tileSetName, tileSetInfo.Description, tileSetInfo.Width, tileSetInfo.Height);
            foreach (var keyword in tileSetInfo.Keywords)
                tileSet.AddKeyword(keyword);
            tileSet.IsComplete = tileSetInfo.IsComplete;
            foreach (var tile in tiles)
            {
                if (tile.TileSetName == tileSetName)
                    tileSet.AddTile(tile);
            }
            return tileSet;
        }

        /// <summary>
        /// Iterate over all of the tiles that match based on a predicate function
        /// </summary>
        /// <param name="predicate">A function: bool predicate(ITile2D) that returns true
        /// if this tile should be enumerated, false otherwise</param>
        /// <returns></returns>
        public IEnumerable<ITile2D> FindTiles(Func<ITile2D, bool> predicate)
        {
            foreach(var tile in Instance.Tiles)
            {
                if (predicate(tile))
                    yield return tile;
            }
        }
    }
}
