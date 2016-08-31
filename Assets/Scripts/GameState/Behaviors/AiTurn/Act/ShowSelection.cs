using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.AiTurn.Act
{
    class ShowSelection : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RegisterWidget(new HighlightAttackWidget(ActiveUnit));
            var targetTile = GameManager.instance.UnitTurn.ActTargetTile;
            GameManager.instance.levelManager.GetMap().SetTileCursor(targetTile.tilePosition, () =>
            {
                RegisterWidget(new UnitInfoWidget(targetTile.GetUnit()));
                animator.SetTrigger("State Complete");
            });
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}