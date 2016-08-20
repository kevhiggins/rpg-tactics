using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.AiTurn.Act
{
    class AiConfirmTarget : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var targetUnit = GameManager.instance.UnitTurn.ActTargetTile.GetUnit();
            RegisterWidget(new TargetActionBoxWidget(ActiveUnit, targetUnit));
            RegisterWidget(new HighlightAttackWidget(ActiveUnit));
            animator.SetTrigger("State Complete");
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}
