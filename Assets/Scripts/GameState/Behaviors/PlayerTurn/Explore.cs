using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.PlayerTurn
{
    class Explore : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RegisterWidget(new ExploreWidget());
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
        }
    }
}
