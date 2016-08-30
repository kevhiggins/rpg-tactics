using System.Collections.Generic;
using Pathfinding;
using Rpg.Map;
using Rpg.Unit;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rpg.Widgets
{
    class HighlightAttackWidget : IWidget
    {
        private List<GameObject> attackTiles;
        public List<TilePosition> AttackTilePositions { get; private set; }

        public HighlightAttackWidget(IUnit unit)
        {
            var map = GameManager.instance.levelManager.GetMap();
            AttackTilePositions = GetAdjacentTilePositions(unit.GetTile().tilePosition);
            AttackTilePositions.Remove(unit.GetTile().tilePosition);


            var levelManager = GameManager.instance.levelManager;

            attackTiles = levelManager.HighlightTiles(AttackTilePositions, levelManager.attackHighlightedTile);
        }

        public void Dispose()
        {
            foreach (var attackTile in attackTiles)
            {
                Object.Destroy(attackTile);
            }
        }

        public void HandleInput()
        {
        }

        protected List<TilePosition> GetAdjacentTilePositions(TilePosition tilePosition)
        {
            var validAdjacentTilePositions = new List<TilePosition>();
            var map = GameManager.instance.levelManager.GetMap();

            var tiles = new List<Tile>();

            tiles.Add(map.GetTile(tilePosition.x - 1, tilePosition.y));
            tiles.Add(map.GetTile(tilePosition.x + 1, tilePosition.y));
            tiles.Add(map.GetTile(tilePosition.x, tilePosition.y - 1));
            tiles.Add(map.GetTile(tilePosition.x, tilePosition.y + 1));

            foreach(var tile in tiles)
            {
                if (tile == null || !tile.IsPassable)
                    continue;
                validAdjacentTilePositions.Add(tile.tilePosition);
            }

            return validAdjacentTilePositions;
        }
    }
}
