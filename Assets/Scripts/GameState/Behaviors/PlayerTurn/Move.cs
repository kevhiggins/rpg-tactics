using System.Collections.Generic;
using Rpg.Map;
using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.PlayerTurn
{
    class Move : AbstractActiveUnitStateBehavior
    {
        private HighlightMoveRangeWidget highlightMoveRangeWidget;

        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            highlightMoveRangeWidget = new HighlightMoveRangeWidget(ActiveUnit);
            RegisterWidget(highlightMoveRangeWidget);
            RegisterWidget(new ExploreWidget());
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (InputManager.Accept())
            {
                var map = GameManager.instance.levelManager.GetMap();
                var cursorTilePosition = map.GetCursorTilePosition();

                // If the tile is not the current user tile, and the tile is in the list of highlighted tiles, then move the unit.
                if (!ActiveUnit.GetTile().tilePosition.Equals(cursorTilePosition))
                {
                    if (UnitCanMoveToTilePosition(cursorTilePosition))
                    {
                        DisposeWidgets();
                        map.MoveUnitToSelectedTile(ActiveUnit, () => { animator.SetTrigger("State Complete"); });
                    }
                }
            }

            if (InputManager.Cancel())
            {
                animator.SetTrigger("Cancel");
            }
        }

        /// <summary>
        /// Returns whether or not the unit can move to the selected tile position.
        /// </summary>
        /// <param name="tilePosition"></param>
        /// <returns></returns>
        protected bool UnitCanMoveToTilePosition(TilePosition tilePosition)
        {
            foreach (var highlightedTilePosition in highlightMoveRangeWidget.HighlightedTilePositions)
            {
                if (tilePosition.Equals(highlightedTilePosition))
                {
                    return true;
                }
            }
            return false;
        }


    }
}