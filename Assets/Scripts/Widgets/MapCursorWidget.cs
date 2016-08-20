using Rpg.Widgets;
using UnityEngine;

namespace Rpg.Widgets
{
    class MapCursorWidget : AbstractWidget
    {
        public delegate void HandleCursorMove();

        public event HandleCursorMove OnCursorMove = () => { };

        public override void Dispose()
        {
        }

        public override void HandleInput()
        {
            var map = GameManager.instance.levelManager.GetMap();
            var inputManager = GameManager.instance.inputManager;

            // Move the tile cursor depending on axis position
            if (inputManager.GetHorizontalDown())
            {
                var axisValue = Input.GetAxis("Horizontal");
                map.MoveTileCursor(axisValue > 0 ? 1 : -1, 0);
                CursorMove();
            }
            else if (inputManager.GetVerticalDown())
            {
                var axisValue = Input.GetAxis("Vertical");
                map.MoveTileCursor(0, axisValue > 0 ? -1 : 1);
                CursorMove();
            }
        }

        protected void CursorMove()
        {
            OnCursorMove();
        }
    }
}