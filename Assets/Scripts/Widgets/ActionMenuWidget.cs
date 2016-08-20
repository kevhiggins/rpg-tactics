using System.Collections.Generic;
using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg.Widgets
{
    class ActionMenuWidget : UiWidget
    {
        private IUnit unit;
        private GameObject menuItems;
        private GameObject activeMenuItem;
        private GameObject cursor;
        private int menuItemCount;
        private int activeMenuItemIndex;
        private List<int> disabledIndexes = new List<int>();

        public ActionMenuWidget(IUnit unit) : base(CreateCanvas())
        {
            this.unit = unit;

            // Find the menuItems game object, and the number if menu items.

            GameObject panel = GameObjectHelper.FindChildByName(canvas, "Panel");

            menuItems = GameObjectHelper.FindChildByName(panel, "Menu Items");
            cursor = GameObjectHelper.FindChildByName(panel, "Hand Cursor");

            menuItemCount = menuItems.transform.childCount;

            if (unit.HasMoved)
            {
                DisableItem("Move");
            }

            if (unit.HasActed)
            {
                DisableItem("Act");
            }

            // Highlight the first menu item.
            // TODO don't start on the first item if it's disabled

            var startIndex = 0;
            while (disabledIndexes.Contains(startIndex))
            {
                startIndex++;
            }

            ActivateMenuItem(startIndex);

            SetMenuPosition(panel);
        }

        protected void DisableItem(string itemName)
        {
            var index = 0;


            foreach (Transform item in menuItems.transform)
            {
                if (item.gameObject.name == itemName)
                {
                    disabledIndexes.Add(index);
                    var textScript = item.gameObject.GetComponent<Text>();
                    textScript.color = Color.grey;
                }
                index++;
            }
        }

        private static GameObject CreateCanvas()
        {
            return Object.Instantiate(GameManager.instance.activeUnitMenu);
        }

        protected void SetMenuPosition(GameObject panel)
        {
            var unitGameObject = unit.GetGameObject();
            var targetPosition = unitGameObject.transform.position;

            var rectTransform = panel.GetComponent<RectTransform>();

            var pixelsToUnits = GameManager.instance.pixelsToUnits;



            targetPosition += new Vector3((rectTransform.GetWidth()/ pixelsToUnits) /2, 0, 0);
            targetPosition += new Vector3((float)20 / pixelsToUnits, rectTransform.GetHeight() / pixelsToUnits / 2, 0);

            panel.transform.position = targetPosition;
        }

        public override void HandleInput()
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
            var menuItemPosition = rectTransform.localPosition;
            var menuPosition = menuItems.GetComponent<RectTransform>().localPosition;


            // Determine the cursor position using the menuPosition + the menuItemPosition - the menuItemWIdth / 2 - the cursor width / 2
            var cursorPosition = menuPosition + menuItemPosition - new Vector3(rect.width/2, 0, 0) -
                                 new Vector3(cursor.GetComponent<RectTransform>().rect.width/2, 0, 0);

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

        public string GetActiveItemName()
        {
            if (disabledIndexes.Contains(activeMenuItemIndex))
            {
                return null;
            }
            return activeMenuItem.name;
        }
    }
}