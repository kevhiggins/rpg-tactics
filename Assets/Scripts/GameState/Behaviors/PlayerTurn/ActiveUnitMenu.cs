﻿using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors.PlayerTurn
{
    /// <summary>
    /// Implements state logic for the active unit's menu and the results of the options.
    /// </summary>
    class ActiveUnitMenu : AbstractActiveUnitStateBehavior
    {
        private ActionMenuWidget actionMenu;

        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Set the tile cursor to the active unit's position.
            GameManager.instance.levelManager.GetMap().SetTileCursor(ActiveUnit.GetTile().tilePosition, Initialize);
        }

        private void Initialize()
        {
            actionMenu = new ActionMenuWidget(ActiveUnit);
            RegisterWidget(actionMenu);
            RegisterWidget(new UnitInfoWidget(ActiveUnit));
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // If the accept command is sent, trigger the currently selected menu item.
            if (InputManager.Accept())
            {
                var activeItemName = actionMenu.GetActiveItemName();
                if (activeItemName != null)
                {
                    animator.SetTrigger(activeItemName);
                }
            }

            if (InputManager.Cancel())
            {
                animator.SetTrigger("Cancel");
            }
        }
    }
}