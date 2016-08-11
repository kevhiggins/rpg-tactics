using UnityEngine;

namespace Rpg.GameState
{

    class TestGameState : IGameState
    {
        public void HandleInput()
        {
            var map = GameManager.instance.levelManager.GetMap();
            var inputManager = GameManager.instance.inputManager;

            // Move the tile cursor depending on axis position
            if (inputManager.GetHorizontalDown())
            {
                var axisValue = Input.GetAxis("Horizontal");
                map.MoveTileCursor(axisValue > 0 ? 1 : -1, 0);
            }
            else if (inputManager.GetVerticalDown())
            {
                var axisValue = Input.GetAxis("Vertical");
                map.MoveTileCursor(0, axisValue > 0 ? -1 : 1);
            }

            if (Input.GetButton("Accept"))
            {
              //  map.MoveUnitToSelectedTile(GameManager.instance.hero);
            }
        }

        public void Enable()
        {
            throw new System.NotImplementedException();
        }

        public void Disable()
        {
            throw new System.NotImplementedException();
        }
    }
}

