using System.Collections.Generic;
using System;
using Rpg.Unit;
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

        /// <summary>
        /// The map width in unity units.
        /// </summary>
        float MapWidth { get; }

        /// <summary>
        /// The map height in unity units.
        /// </summary>
        float MapHeight { get; }

        /// <summary>
        /// Array of tiles on the map indexed by their x and y positions.
        /// </summary>
        Tile[,] Tiles { get; }

        /// <summary>
        /// The list of units on the map.
        /// </summary>
        List<IUnit> Units { get; }

        void SetMap(Map map);
        void ProcessTileData();
    }
}