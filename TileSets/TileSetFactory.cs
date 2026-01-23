using System.Collections.Generic;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <summary>
    /// Factory to create an ITileSet
    /// </summary>
    public class TileSetFactory
    {
        private TileFactory _factory;
        private string _tileSetName;
        private TileSet _tileSet;
        private List<(int,int,int,int)> _edgeColors = new List<(int, int, int, int)> ();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tileSetName">A name for newly constructed tile set.</param>
        /// <param name="description">A tile set description.</param>
        /// <param name="tileWidth">The width of the tiles in the tileset.</param>
        /// <param name="tileHeight">the height of the tiles in the tileset.</param>
        /// <param name="initialID">An initial tile ID (defaults to zero). Subsequent tiles will increment this to give unique tile id's.</param>
        public TileSetFactory(string tileSetName, string description, int tileWidth, int tileHeight, int initialID = 0)
        {
            _tileSetName = tileSetName;
            _factory = new TileFactory(tileSetName, initialID);
            _tileSet = new TileSet(tileSetName, description, tileWidth, tileHeight);
        }

        /// <summary>
        /// Add a new tile to the tileset. Handles unique id's and tileset binding.
        /// </summary>
        /// <param name="left">The "color" associated with the left edge.</param>
        /// <param name="top">The "color" associated with the top edge.</param>
        /// <param name="right">The "color" associated with the right edge.</param>
        /// <param name="bottom">The "color" associated with the bottom edge.</param>
        public void AddTile(int left, int top, int right, int bottom)
        {
            var tile = _factory.CreateTile(left, top, right, bottom);
            _tileSet.AddTile(tile);
        }

        /// <summary>
        /// Add a new tile to the tileset only if one with the same edge colors does not exist.
        /// </summary>
        /// <param name="left">The "color" associated with the left edge.</param>
        /// <param name="top">The "color" associated with the top edge.</param>
        /// <param name="right">The "color" associated with the right edge.</param>
        /// <param name="bottom">The "color" associated with the bottom edge.</param>
        public void AddTileIfUniqueColors(int left, int top, int right, int bottom)
        {
            if (!_edgeColors.Contains((left,top,right,bottom)))
            {
                AddTile(left, top, right, bottom);
                _edgeColors.Add((left,top,right,bottom));
            }
        }

        /// <summary>
        /// Get the constructed tile set from the factory.
        /// </summary>
        /// <returns></returns>
        public ITileSet GetTileSet() { return _tileSet; }

        /// <summary>
        /// Set the tile sets Default tile.
        /// </summary>
        /// <param name="left">The "color" associated with the left edge.</param>
        /// <param name="top">The "color" associated with the top edge.</param>
        /// <param name="right">The "color" associated with the right edge.</param>
        /// <param name="bottom">The "color" associated with the bottom edge.</param>
        /// <param name="additionallyAddToTileSet">If true adds the tile to the tile set as well.</param>
        public void SetDefaultTile(int left, int top, int right, int bottom, bool additionallyAddToTileSet)
        {
            var tile = _factory.CreateTile(left, top, right, bottom);
            _tileSet.SetDefaultTile(tile);
            if (additionallyAddToTileSet)
                _tileSet.AddTile(tile);
        }
    }
}