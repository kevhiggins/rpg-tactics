using System;

namespace Rpg.Widgets
{
    public interface IWidget : IDisposable
    {
        void HandleInput();
    }
}
