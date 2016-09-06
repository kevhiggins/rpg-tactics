using UnityEngine;
using System.Linq;

namespace Rpg.GameState.Behaviors.AiTurn.Move
{
    class AiMove : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var movementPath = GameManager.instance.UnitTurn.MovementPath;
            var finalPosition = movementPath.Last();

             var tile = finalPosition.Tile;

            ActiveUnit.MoveToTile(tile, () => { animator.SetTrigger("State Complete"); });
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}