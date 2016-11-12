using System.Collections.Generic;
using System.Runtime.InteropServices;
using GraphPathfinding;
using Rpg.Map;
using Rpg.PathFinding;
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
            AttackTilePositions = GetAdjacentTilePositions(unit);
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

        protected List<TilePosition> GetAdjacentTilePositions(IUnit unit)
        {
            var validAdjacentTilePositions = new List<TilePosition>();

            var pathFinder = new AStarPathfinder();
            var nodesInRange = pathFinder.FindNodesInCostRange(unit.GetTile().GraphNode, unit.AttackRange);

            var tiles = new List<Tile>();
            foreach (var node in nodesInRange)
            {
                var tileNode = (GraphNodeTile)node;
                tiles.Add(tileNode.Tile);
            }

            foreach (var tile in tiles)
            {
                if (tile == null || !tile.IsPassable)
                    continue;
                validAdjacentTilePositions.Add(tile.tilePosition);
            }

            return validAdjacentTilePositions;
        }
    }
}
