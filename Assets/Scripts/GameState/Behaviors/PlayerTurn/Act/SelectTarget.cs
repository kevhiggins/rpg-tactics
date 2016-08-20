using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.PlayerTurn.Act
{
    class SelectTarget : AbstractActiveUnitStateBehavior
    {
        private HighlightAttackWidget highlightAttackWidget;

        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            highlightAttackWidget = new HighlightAttackWidget(ActiveUnit);
            RegisterWidget(highlightAttackWidget);
            RegisterWidget(new ExploreWidget());
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (InputManager.Accept())
            {
                // Check if a unit exists on the tile and if so go to the TargetConfirmation state
                var map = GameManager.instance.levelManager.GetMap();
                var selectedTile = map.GetSelectedTile();

                if (highlightAttackWidget.AttackTilePositions.Contains(selectedTile.tilePosition) && selectedTile.HasUnit())
                {
                    GameManager.instance.battleManager.TargetUnit = selectedTile.GetUnit();
                    animator.SetTrigger("Accept");
                }
            }

            if (InputManager.Cancel())
            {
                animator.SetTrigger("Cancel");
            }
        }
    }
}
