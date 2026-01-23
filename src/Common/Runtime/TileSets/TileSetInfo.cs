using System.Collections.Generic;

namespace CrawfisSoftware.Tiling
{
    // Internal structore for a database entry
    internal struct TileSetInfo
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public int Count { get; private set; }
        public bool IsComplete { get; private set; }
        public IList<string> Keywords { get; private set; }
        public static TileSetInfo Null { get; internal set; }
        static TileSetInfo()
        {
            Null = new TileSetInfo("Non-existant", "", 0, -1, -1, false, new string[]{ "" });
        }

        public TileSetInfo(string name, string description, int count, float width, float height, bool isComplete, IEnumerable<string> keywords) {
            Name = name;
            Description = description;
            Count = count;
            Width = width;
            Height = height;
            IsComplete = isComplete;
            List<string> copyOfKeywords = new List<string>();
            foreach (string keyword in keywords)
                copyOfKeywords.Add(keyword);
            copyOfKeywords.TrimExcess();
            Keywords = copyOfKeywords;
        }
    }
}