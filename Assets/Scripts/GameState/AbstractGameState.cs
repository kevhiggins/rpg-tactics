using Rpg.GameState;
using UnityEngine;

namespace Rpg.GameState
{
    /// <summary>
    /// Abstract Game state that handles actions that can take place in most map game states.
    /// </summary>
    public abstract class AbstractGameState : IGameState
    {
        public virtual void HandleInput()
        {
            var inputManager = GameManager.instance.inputManager;

            if (inputManager.Accept())
            {
                HandleAccept();
            }

            if (inputManager.Cancel())
            {
                HandleCancel();
            }
        }

        public abstract void HandleAccept();
        public abstract void HandleCancel();

        public abstract void Enable();
        public abstract void Disable();
    }
}
