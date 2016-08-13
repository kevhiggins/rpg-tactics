using Rpg.Widgets;
using UnityEngine;

namespace Rpg.Widgets
{
    public abstract class UiWidget : AbstractWidget
    {
        protected GameObject canvas;

        public UiWidget(GameObject canvas)
        {
            this.canvas = canvas;
            var canvasScript = canvas.GetComponent<Canvas>();
            canvasScript.worldCamera = Camera.main;
        }
    }
}
