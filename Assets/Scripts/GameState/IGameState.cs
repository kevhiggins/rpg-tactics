namespace Rpg.GameState
{
    public interface IGameState
    {
        void HandleInput();
        void Enable();
        void Disable();
    }
}

