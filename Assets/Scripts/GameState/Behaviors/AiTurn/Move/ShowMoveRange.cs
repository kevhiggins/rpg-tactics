using System.Linq;
using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.AiTurn.Move
{
    class ShowMoveRange : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RegisterWidget(new HighlightMoveRangeWidget(ActiveUnit));

            var movementPath = GameManager.instance.UnitTurn.MovementPath;
            var finalPosition = movementPath.Last();

            var tile = finalPosition.Tile;
            GameManager.instance.levelManager.GetMap().SetTileCursor(tile.tilePosition, () =>
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