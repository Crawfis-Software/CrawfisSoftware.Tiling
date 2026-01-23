using CrawfisSoftware.Tiling;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrawfisSoftware.Tiling.TileSets
{
    /// <inheritdoc/>
    public class TileSet : ITileSet
    {
        private IList<string> _keywordsList;
        /// <summary>
        /// Protected. The list of tiles in this tileSet.
        /// </summary>
        protected IList<ITile2D> m_tileSet = new List<ITile2D>();

        /// <inheritdoc/>
        public string Name { get; set; }


        /// <inheritdoc/>
        public string Description { get; set; }

        /// <inheritdoc/>
        public int Count
        {
            get { return m_tileSet.Count; }
        }

        /// <inheritdoc/>
        public bool IsComplete { get; set; } = false;

        /// <inheritdoc/>
        public float Width { get; set; }

        /// <inheritdoc/>
        public float Height { get; set; }

        /// <inheritdoc/>
        public IEnumerable<string> Keywords
        {
            get
            {
                return _keywordsList.AsEnumerable<string>();
            }
        }
        /// <inheritdoc/>
        public ITile2D DefaultTile { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name for this tileSet</param>
        /// <param name="description">A short description of the tileSet</param>
        /// <param name="width">The width of the tile</param>
        /// <param name="height">The height of the tile</param>
        public TileSet(string name, string description, float width, float height)
        {
            Name = name;
            Description = description;
            Width = width;
            Height = height;
            _keywordsList = new List<string>();
        }

        /// <summary>
        /// Add a new keyword to this tileSet.
        /// </summary>
        /// <param name="keyword">A new keyword (string)</param>
        public void AddKeyword(string keyword)
        {
            _keywordsList.Add(keyword);
        }

        /// <inheritdoc/>
        public ITile2D GetTile(int tileID)
        {
            foreach (var tile in from tile in m_tileSet
                                 where tile.ID == tileID
                                 select tile)
            {
                return tile;
            }

            return DefaultTile;
        }

        /// <inheritdoc/>
        public IList<ITile2D> GetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice)
        {
            var list = m_tileSet.Where<ITile2D>(TileSearchUtility.MeetsConstraints(leftChoice, topChoice, rightChoice, bottomChoice));
            return list.ToList<ITile2D>();
        }

        /// <inheritdoc/>
        public bool TryGetMatchingTiles(int leftChoice, int topChoice, int rightChoice, int bottomChoice, out IList<ITile2D> tiles)
        {
            var list = m_tileSet.Where<ITile2D>(TileSearchUtility.MeetsConstraints(leftChoice, topChoice, rightChoice, bottomChoice));
            tiles = list.ToList<ITile2D>();
            if (tiles.Count > 0)
                return true;
            return false;
        }

        /// <summary>
        /// Add a new tile to the tileSet
        /// </summary>
        /// <param name="tile">The tile to add</param>
        public void AddTile(ITile2D tile)
        {
            m_tileSet.Add(tile);
        }

        /// <summary>
        /// Set the default tile for this tileSet
        /// </summary>
        /// <param name="tile"></param>
        public void SetDefaultTile(ITile2D tile)
        {
            DefaultTile = tile;
        }

        /// <summary>
        /// Set the default tile for this tileSet
        /// </summary> 
        public void SetDefaultTile(int index)
        {
            DefaultTile = m_tileSet[index];
        }

        /// <inheritdoc/>
        public IEnumerator<ITile2D> GetEnumerator()
        {
            return m_tileSet.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
