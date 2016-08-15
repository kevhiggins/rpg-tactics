using Rpg.GameState;
using UnityEngine;

namespace Assets.Scripts.GameState
{
    /// <summary>
    /// Abstract Game state that handles actions that can take place in most map game states.
    /// </summary>
    abstract class AbstractMapGameState : AbstractGameState
    {
        public override void HandleInput()
        {
            base.HandleInput();

            var map = GameManager.instance.levelManager.GetMap();
            var inputManager = GameManager.instance.inputManager;

            // Move the tile cursor depending on axis position
            if (inputManager.GetHorizontalDown())
            {
                var axisValue = Input.GetAxis("Horizontal");
                map.MoveTileCursor(axisValue > 0 ? 1 : -1, 0);
                HandleCursorMove();
            }
            else if (inputManager.GetVerticalDown())
            {
                var axisValue = Input.GetAxis("Vertical");
                map.MoveTileCursor(0, axisValue > 0 ? -1 : 1);
                HandleCursorMove();
            }
        }

        public abstract void HandleCursorMove();
    }
}
