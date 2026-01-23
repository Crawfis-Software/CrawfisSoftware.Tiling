using CrawfisSoftware.Tiling;

namespace CrawfisSoftware.Tiling
{
    /// <summary>
    /// Interface for tilings to render individual tiles.
    /// </summary>
    public interface IDrawableTile<TContext, TRepresentation>
    {
        /// <summary>
        /// Draw the tile at tiling location. Extra data can be passed in the Context.
        /// </summary>
        /// <param name="tileLocation"></param>
        /// <param name="Context">A placeholder for a concrete implementation to pass in data.</param>
        /// <returns>A render specific output. A placeholder for a concrete implementation to return data.</returns>
        TRepresentation Render(Tiling2DIndex tileLocation, TContext Context);

        /// <summary>
        /// This is called before the tile is rendered, and perhaps before any tiles are rendered.
        /// </summary>
        /// <param name="Context">A placeholder for a concrete implementation to pass in data.</param>
        /// <returns>A render specific output. A placeholder for a concrete implementation to return data.</returns>
        /// <remarks>PreRender may be used to load or set textures, build the tileset or as in the case of the
        /// SVGTile, create the SVG code that can be instanced in the tiling.</remarks>
        TRepresentation PreRender(TContext Context);
    }
}
