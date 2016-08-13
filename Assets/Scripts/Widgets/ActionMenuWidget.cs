using Assets.Scripts.GameState;
using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;

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

        public ActionMenuWidget(IUnit unit) : base(CreateCanvas())
        {
            this.unit = unit;

            // Find the menuItems game object, and the number if menu items.

            GameObject panel = GameObjectHelper.FindChildByName(canvas, "Panel");

            menuItems = GameObjectHelper.FindChildByName(panel, "Menu Items");
            cursor = GameObjectHelper.FindChildByName(panel, "Hand Cursor");

            menuItemCount = menuItems.transform.childCount;

            // Highlight the first menu item.
            ActivateMenuItem(0);

            SetMenuPosition(panel);
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

        public override void Dispose()
        {
            Object.Destroy(canvas);
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

            // If the accept command is sent, trigger the currently selected menu item.
            if (inputManager.Accept())
            {
                TriggerCurrentMenuItem();
            }

            if (inputManager.Cancel())
            {
                GameManager.instance.GameState = new ExploreMapState(unit);
            }
        }

        private static GameObject CreateCanvas()
        {
            return Object.Instantiate(GameManager.instance.activeUnitMenu);
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


        public void TriggerCurrentMenuItem()
        {
            // If the Move option is selected, then switch to the SelectUnitMovement state.
            if (activeMenuItem.name == "Move")
            {
                GameManager.instance.GameState = new SelectUnitMovementState(unit);
            }
        }
    }
}