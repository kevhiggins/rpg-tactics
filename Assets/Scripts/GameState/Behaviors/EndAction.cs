using UnityEngine;

namespace Rpg.GameState.Behaviors
{
    class EndAction : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var hasActions = !ActiveUnit.HasActed || !ActiveUnit.HasMoved;
            animator.SetBool("Has Actions", !ActiveUnit.HasActed || !ActiveUnit.HasMoved);
            if (!hasActions)
            {
                ActiveUnit.EndTurn();
            }

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
