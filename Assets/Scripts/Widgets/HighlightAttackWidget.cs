using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpg.Map;
using Rpg.Unit;
using Rpg.Widgets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Widgets
{
    class HighlightAttackWidget : IWidget
    {
        private List<GameObject> attackTiles;
        private List<TilePosition> attackTilePositions;
        private IUnit unit;

        public HighlightAttackWidget(IUnit unit)
        {
            this.unit = unit;
            var attackRange = 1;
            var map = GameManager.instance.levelManager.GetMap();
            attackTilePositions = map.GetTilePositionsInRange(unit.GetTile().tilePosition, attackRange);
            attackTilePositions.Remove(unit.GetTile().tilePosition);


            var levelManager = GameManager.instance.levelManager;

            attackTiles = levelManager.HighlightTiles(attackTilePositions, levelManager.attackHighlightedTile);
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
