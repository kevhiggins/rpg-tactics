using System.Collections.Generic;
using Assets.Scripts.GameState;
using Assets.Scripts.Widgets;
using Rpg.Map;
using Rpg.Unit;
using Rpg.Widgets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rpg.GameState
{
    class AttackState : ExploreMapState
    {
        private IUnit unit;
        private List<GameObject> attackTiles;
        private List<TilePosition> attackTilePositions;
        private IWidget highlightAttackWidget;

        public AttackState(IUnit unit) : base(unit)
        {
            this.unit = unit;
            highlightAttackWidget = new HighlightAttackWidget(unit);
        }

        public override void Disable()
        {
            base.Disable();
            highlightAttackWidget.Dispose();
        }

        public override void HandleAccept()
        {
            var map = GameManager.instance.levelManager.GetMap();
            
            // Check if a unit exists on the tile and if so go to the TargetConfirmation state
            var selectedTile = map.GetSelectedTile();
            if (selectedTile.HasUnit())
            {
                // If so bring up the two unit info screens state
                GameManager.instance.GameState = new TargetConfirmationState(unit, selectedTile.GetUnit());
            }
        }

        public override void HandleCancel()
        {
            GameManager.instance.GameState = new ActiveUnitMenuState(unit);
        }
    }
}
