using Rpg.Unit;
using UnityEngine;

namespace Rpg.GameState.Behaviors
{
    class AdvanceEventQueue : AbstractGameStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var actionQueue = GameManager.instance.actionQueue;

            IUnit unit;
            do
            {
                actionQueue.ClockTick();
            } while ((unit = actionQueue.GetActiveUnit()) == null);

            // At this point we will have an active unit. Select the unit, and prepare for user input.
            unit.StartTurn();

            if (unit is IFriendlyUnit)
            {
                animator.SetTrigger("Player Turn");
            }
            else
            {
                animator.SetTrigger("Enemy Turn");
            }
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}