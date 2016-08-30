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

            // Get the active unit. If there is no valid active unit, increment time, until one is found.
            while((unit = actionQueue.GetActiveUnit()) == null)
            {
                actionQueue.ClockTick();
            }

            // Start the active units turn.
            GameManager.instance.UnitTurn = new UnitTurn(unit);

            animator.SetTrigger(unit.IsAi == false ? "Player Turn" : "AI Turn");
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}