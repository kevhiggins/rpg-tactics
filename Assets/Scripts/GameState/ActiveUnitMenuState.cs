using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpg.GameState;
using Rpg.Unit;

namespace Assets.Scripts.GameState
{
    class ActiveUnitMenuState : IGameState
    {
        private IUnit activeUnit;

        public ActiveUnitMenuState(IUnit unit)
        {
            activeUnit = unit;
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
