using System;
using Rpg.GameState;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Assets.Scripts.Unity;
using Rpg.Widgets;

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
        private GameObject cursor;
        private IWidget unitInfo;


        public ActiveUnitMenuState(IUnit unit)
        {
            activeUnit = unit;
            activeUnitMenu = Object.Instantiate(GameManager.instance.activeUnitMenu);

            // Set the camera for the canvas.
            var canvasScript = activeUnitMenu.GetComponent<Canvas>();
            canvasScript.worldCamera = Camera.main;

            // Find the menuItems game object, and the number if menu items.

            GameObject panel = GameObjectHelper.FindChildByName(activeUnitMenu, "Panel");

            menuItems = GameObjectHelper.FindChildByName(panel, "Menu Items");
            cursor = GameObjectHelper.FindChildByName(panel, "Hand Cursor");        

           // menuItems = activeUnitMenu.transform.GetChild(0).GetChild(1).gameObject;
            menuItemCount = menuItems.transform.childCount;

            unitInfo = new UnitInfoWidget(unit);

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

            if (inputManager.Cancel())
            {
                GameManager.instance.GameState = new ExploreMapState(activeUnit);
            }
        }

        public void Enable()
        {
            activeUnitMenu.SetActive(true);

            // Move the tile cursor to the active unit.
            GameManager.instance.levelManager.GetMap().SetTileCursor(activeUnit.GetTile().tilePosition);
        }

        public void Disable()
        {
            activeUnitMenu.SetActive(false);
            Object.Destroy(activeUnitMenu);
            unitInfo.Dispose();
        }

        public void ActivateMenuItem(int menuItemIndex)
        {
            if (activeMenuItem != null)
            {
                DeactiveMenuItem(activeMenuItem);
            }

            var menuItem = menuItems.transform.GetChild(menuItemIndex).gameObject;


            var rectTransform = menuItem.GetComponent<RectTransform>();
            var rect = rectTransform.rect;
            var menuItemPosition = (Vector3) rectTransform.localPosition;
            var menuPosition = (Vector3) menuItems.GetComponent<RectTransform>().localPosition;


            // Determine the cursor position using the menuPosition + the menuItemPosition - the menuItemWIdth / 2 - the cursor width / 2
            var cursorPosition = menuPosition + menuItemPosition - new Vector3(rect.width/2, 0, 0) - new Vector3(cursor.GetComponent<RectTransform>().rect.width / 2, 0, 0);

            // Offset the cursor slightly to move it away from the menu item, and make the finger point more at the middle.
            cursorPosition += new Vector3(-2, -3, 0);


            // Set the cursor position on the cursor game object's RectTransform
            cursor.GetComponent<RectTransform>().localPosition = cursorPosition;

            activeMenuItem = menuItem;
            activeMenuItemIndex = menuItemIndex;
        }

        public void DeactiveMenuItem(GameObject menuItem)
        {
            // Do nothing. The cursor is moved, so there is nothing to deactivate.
        }


        public void TriggerCurrentMenuItem()
        {
            // If the Move option is selected, then switch to the SelectUnitMovement state.
            if (activeMenuItem.name == "Move")
            {
                GameManager.instance.GameState = new SelectUnitMovementState(activeUnit);
            }
        }
    }
}