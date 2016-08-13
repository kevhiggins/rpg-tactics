using System;

namespace Rpg.Widgets
{
    interface IWidget : IDisposable
    {
        void HandleInput();
    }
}
