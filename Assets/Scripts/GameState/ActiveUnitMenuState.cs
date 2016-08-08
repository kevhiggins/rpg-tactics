using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpg.GameState;
using Rpg.Unit;
using UnityEngine;

namespace Assets.Scripts.GameState
{
    class ActiveUnitMenuState : IGameState
    {
        private IUnit activeUnit;
        private GameObject activeUnitMenu;

        public ActiveUnitMenuState(IUnit unit)
        {
            activeUnit = unit;
            activeUnitMenu = GameManager.Instantiate(GameManager.instance.activeUnitMenu) as GameObject;
        }

        public void HandleInput()
        {
            
            // Display Menu
            // Move
            // Act
            // Wait
        }
    }
}
