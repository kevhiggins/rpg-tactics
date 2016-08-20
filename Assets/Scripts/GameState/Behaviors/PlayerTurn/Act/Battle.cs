using UnityEngine;

namespace Rpg.GameState.Behaviors.PlayerTurn.Act
{
    class Battle : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var targetUnit = GameManager.instance.battleManager.TargetUnit;
            GameManager.instance.battleManager.AttackUnit(ActiveUnit, targetUnit, () =>
            {
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
