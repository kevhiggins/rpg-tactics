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
        private IUnit unit;

        private IWidget actionMenu;
        private IWidget unitInfo;


        public ActiveUnitMenuState(IUnit unit)
        {
            this.unit = unit;
            actionMenu = new ActionMenuWidget(unit);
            unitInfo = new UnitInfoWidget(unit);
        }
     

        public void HandleInput()
        {
            actionMenu.HandleInput();
        }

        public void Enable()
        {
            // Move the tile cursor to the active unit.
            GameManager.instance.levelManager.GetMap().SetTileCursor(unit.GetTile().tilePosition);
        }

        public void Disable()
        {
            actionMenu.Dispose();
            unitInfo.Dispose();
        }
    }
}