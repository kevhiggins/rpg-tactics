
using Assets.Scripts.Widgets;
using Rpg.GameState;
using Rpg.Unit;
using Rpg.Widgets;

namespace Assets.Scripts.GameState
{
    class TargetConfirmationState : AbstractGameState
    {
        private IUnit sourceUnit;
        private IUnit targetUnit;
        private IWidget targetActionBoxWidget;
        private IWidget highlightAttackWidget;

        public TargetConfirmationState(IUnit sourceUnit, IUnit targetUnit)
        {
            this.sourceUnit = sourceUnit;
            this.targetUnit = targetUnit;

            targetActionBoxWidget = new TargetActionBoxWidget(sourceUnit, targetUnit);
            highlightAttackWidget = new HighlightAttackWidget(sourceUnit);
        }

        public override void Enable()
        {
        }

        public override void Disable()
        {
            targetActionBoxWidget.Dispose();
            highlightAttackWidget.Dispose();
        }

        public override void HandleAccept()
        {
            GameManager.instance.battleManager.AttackUnit(sourceUnit, targetUnit);
        }

        public override void HandleCancel()
        {
            GameManager.instance.GameState = new AttackState(sourceUnit);
        }
    }
}
