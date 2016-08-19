using Assets.Scripts.GameState;
using Rpg.Unit;
using Rpg.Widgets;

namespace Rpg.GameState
{
    class AttackState : ExploreMapState
    {
        private IUnit unit;
        private HighlightAttackWidget highlightAttackWidget;

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
            
            if (highlightAttackWidget.AttackTilePositions.Contains(selectedTile.tilePosition) && selectedTile.HasUnit())
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
