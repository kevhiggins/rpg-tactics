using System;
using Rpg.GameState;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.GameState
{
    class ActiveUnitMenuState : IGameState
    {
        private IUnit activeUnit;
        private GameObject activeUnitMenu;
        private GameObject menuItems;
        private GameObject activeMenuItem;
        private int activeMenuItemIndex;
        private int menuItemCount;
        

        public ActiveUnitMenuState(IUnit unit)
        {
            activeUnit = unit;
            activeUnitMenu = Object.Instantiate(GameManager.instance.activeUnitMenu);

            // Set the camera for the canvas.
            var canvasScript = activeUnitMenu.GetComponent<Canvas>();
            canvasScript.worldCamera = Camera.main;

            // Find the menuItems game object, and the number if menu items.
            menuItems = activeUnitMenu.transform.GetChild(0).GetChild(1).gameObject;
            menuItemCount = menuItems.transform.childCount;

            // Highlight the first menu item.
            ActivateMenuItem(0);
        }

        public void HandleInput()
        {
            var oldMenuItemIndex = activeMenuItemIndex;
            var menuItemIndex = activeMenuItemIndex;


            var inputManager = GameManager.instance.inputManager;

            // Handle moving the highlighted menu item up and down.
            if (inputManager.Up())
            {
                menuItemIndex = (activeMenuItemIndex - 1);
                if (menuItemIndex < 0)
                {
                    menuItemIndex = menuItemCount - 1;
                }
            }
            else if (inputManager.Down())
            {
                menuItemIndex = (activeMenuItemIndex + 1)%menuItemCount;
            }

            // Only try to highlight the item if it's a new item.
            if (oldMenuItemIndex != menuItemIndex)
            {
                ActivateMenuItem(menuItemIndex);
            }

            // If the accept command is sent, trigger the currently selected menu item.
            if (inputManager.Accept())
            {
                TriggerCurrentMenuItem();
            }
        }

        public void Enable()
        {
            GC.Collect();
            activeUnitMenu.SetActive(true);
        }

        public void Disable()
        {
            activeUnitMenu.SetActive(false);
        }

        public void ActivateMenuItem(int menuItemIndex)
        {
            if (activeMenuItem != null)
            {
                DeactiveMenuItem(activeMenuItem);
            }
            
            var menuItem = menuItems.transform.GetChild(menuItemIndex).gameObject;
            var textScript = menuItem.GetComponent<Text>();
            textScript.fontStyle = FontStyle.Italic;
            //            textScript.fontSize = 10;
            //            textScript.OnRebuildRequested();
            activeMenuItem = menuItem;
            activeMenuItemIndex = menuItemIndex;
        }

        public void DeactiveMenuItem(GameObject menuItem)
        {
            var textScript = menuItem.GetComponent<Text>();
            textScript.fontStyle = FontStyle.Normal;
        }


        public void TriggerCurrentMenuItem()
        {
            // If the Move option is selected, then switch to the SelectUnitMovement state.
            if (activeMenuItem.name == "Move")
            {
                GameManager.instance.GameState = new SelectUnitMovementState(this, activeUnit);
            }
        }
    }
}
