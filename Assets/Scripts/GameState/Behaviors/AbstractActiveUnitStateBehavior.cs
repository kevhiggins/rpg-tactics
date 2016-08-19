using Rpg.Unit;
using UnityEngine;

namespace Rpg.GameState.Behaviors
{
    public abstract class AbstractActiveUnitStateBehavior : AbstractGameStateBehavior
    {
        /// <summary>
        /// The unit whose turn it currently is.
        /// </summary>
        protected IUnit ActiveUnit { get; private set; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ActiveUnit = GameManager.instance.actionQueue.GetActiveUnit();
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }
    }
}
