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
            var attackRange = 1;
            var map = GameManager.instance.levelManager.GetMap();
            AttackTilePositions = map.GetTilePositionsInRange(unit.GetTile().tilePosition, attackRange, PathNNConstraint.Default);
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
    }
}
