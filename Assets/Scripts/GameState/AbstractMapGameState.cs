using Rpg.GameState;
using UnityEngine;

namespace Assets.Scripts.GameState
{
    /// <summary>
    /// Abstract Game state that handles actions that can take place in most map game states.
    /// </summary>
    abstract class AbstractMapGameState : IGameState
    {
        public void HandleInput()
        {
//            var levelManager = GameManager.instance.levelManager;
//            var inputManager = GameManager.instance.inputManager;
//
//            // Move the tile cursor depending on axis position
//            if (inputManager.GetHorizontalDown())
//            {
//                var axisValue = Input.GetAxis("Horizontal");
//                levelManager.MoveTileCursor(axisValue > 0 ? 1 : -1, 0);
//            }
//            else if (inputManager.GetVerticalDown())
//            {
//                var axisValue = Input.GetAxis("Vertical");
//                levelManager.MoveTileCursor(0, axisValue > 0 ? -1 : 1);
//            }
//
//            if (Input.GetButton("Accept"))
//            {
//                HandleAccept();
//            }
        }

        public abstract void HandleAccept();
    }
}
