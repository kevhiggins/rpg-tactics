using UnityEngine;

namespace Rpg.Map
{
    public interface ITileMap
    {
        /// <summary>
        /// The game object associated with the tile map.
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        /// Width of map tiles in unity units.
        /// </summary>
        /// <returns></returns>
        float TileWidth { get; }

        /// <summary>
        /// Height of map tiles in unity units.
        /// </summary>
        /// <returns></returns>
        float TileHeight { get; }

        /// <summary>
        /// Number of tiles wide the map has.
        /// </summary>
        /// <returns></returns>
        int TilesWide { get; }

        /// <summary>
        /// Number of tiles high the map has.
        /// </summary>
        /// <returns></returns>
        int TilesHigh { get; }
    }
}