using System.Collections.Generic;
using Assets.Scripts.GameState;
using Rpg.Map;
using Rpg.Unit;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rpg.GameState
{
    class AttackState : ExploreMapState
    {
        private IUnit unit;
        private List<GameObject> attackTiles;
        private List<TilePosition> attackTilePositions;

        public AttackState(IUnit unit) : base(unit)
        {
            this.unit = unit;
            var attackRange = 1;
            var map = GameManager.instance.levelManager.GetMap();
            attackTilePositions = map.GetTilePositionsInRange(unit.GetTile().tilePosition, attackRange);
            attackTilePositions.Remove(unit.GetTile().tilePosition);


            var levelManager = GameManager.instance.levelManager;

            attackTiles = levelManager.HighlightTiles(attackTilePositions, levelManager.attackHighlightedTile);
        }

        public override void Disable()
        {
            base.Disable();
            foreach (var attackTile in attackTiles)
            {
                Object.Destroy(attackTile);
            }
        }

        public override void HandleAccept()
        {
            // Check if a unit exists on the tile
            // If so bring up the two unit info screens state
        }

        public override void HandleCancel()
        {
            GameManager.instance.GameState = new ActiveUnitMenuState(unit);
        }
    }
}
