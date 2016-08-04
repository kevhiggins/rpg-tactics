using UnityEngine;

namespace Rpg.GameState
{

    class TestGameState : IGameState
    {
        public void HandleInput()
        {
            var levelManager = GameManager.instance.levelManager;
            var inputManager = GameManager.instance.inputManager;

            // Move the tile cursor depending on axis position
            if (inputManager.GetHorizontalDown())
            {
                var axisValue = Input.GetAxis("Horizontal");
                levelManager.MoveTileCursor(axisValue > 0 ? 1 : -1, 0);
            }
            else if (inputManager.GetVerticalDown())
            {
                var axisValue = Input.GetAxis("Vertical");
                levelManager.MoveTileCursor(0, axisValue > 0 ? -1 : 1);
            }

            if (Input.GetButton("Accept"))
            {
                levelManager.MoveHeroToSelectedTile(GameManager.instance.hero);
            }
        }
    }
}

