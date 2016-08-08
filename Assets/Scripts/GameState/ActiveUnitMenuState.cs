using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpg.GameState;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

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
            activeUnitMenu = UnityEngine.Object.Instantiate(GameManager.instance.activeUnitMenu);
            menuItems = activeUnitMenu.transform.GetChild(0).GetChild(1).gameObject;
           // menuItems = GameObject.Find("/ActiveUnitMenu/Panel/Menu Items");

            menuItemCount = menuItems.transform.childCount;

            ActivateMenuItem(0);
        }

        public void HandleInput()
        {
            var oldMenuItemIndex = activeMenuItemIndex;
            var menuItemIndex = activeMenuItemIndex;


            var inputManager = GameManager.instance.inputManager;
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

            if (oldMenuItemIndex != menuItemIndex)
            {
                ActivateMenuItem(menuItemIndex);
            }

            // Display Menu
            // Move
            // Act
            // Wait
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
    }
}
