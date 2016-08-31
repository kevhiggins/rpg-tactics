using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors
{
    class Wait : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ActiveUnit.Wait();
            RegisterWidget(new UnitInfoWidget(ActiveUnit));
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
