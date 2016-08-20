using System.Collections.Generic;
using Rpg.Widgets;
using UnityEngine;

namespace Rpg.GameState.Behaviors
{
    public abstract class AbstractGameStateBehavior : StateMachineBehaviour
    {
        protected InputManager InputManager { get; private set; }
        private List<IWidget> widgets = new List<IWidget>();

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            InputManager = GameManager.instance.inputManager;
            Enable(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Disable(animator, stateInfo, layerIndex);
            DisposeWidgets();
        }

        protected void DisposeWidgets()
        {
            // Dispose of all registered widgets
            foreach (var widget in widgets)
            {
                widget.Dispose();
            }
            widgets.Clear();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var widget in widgets)
            {
                widget.HandleInput();
            }
            StateUpdate(animator, stateInfo, layerIndex);
        }

        /// <summary>
        /// Handles automatically calling HandleInput, and disposing the widget
        /// </summary>
        /// <param name="widget"></param>
        protected void RegisterWidget(IWidget widget)
        {
            widgets.Add(widget);
        }

        public abstract void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        public abstract void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
        public abstract void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    }
}