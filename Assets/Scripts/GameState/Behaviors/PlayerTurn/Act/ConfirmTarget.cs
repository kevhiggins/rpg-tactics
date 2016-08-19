using Rpg.Unit;
using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.PlayerTurn.Act
{
    class ConfirmTarget : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var targetUnit = GameManager.instance.battleManager.TargetUnit;
            RegisterWidget(new TargetActionBoxWidget(ActiveUnit, targetUnit));
            RegisterWidget(new HighlightAttackWidget(ActiveUnit));
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (InputManager.Accept())
            {
                animator.SetTrigger("Accept");
            }

            if (InputManager.Cancel())
            {
                animator.SetTrigger("Cancel");
            }
        }
    }
}
