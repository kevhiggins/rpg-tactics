using Rpg.Unit;
using Rpg.Widgets;

namespace Assets.Scripts.GameState
{
    class ExploreMapState : AbstractMapGameState
    {
        private IUnit unit;
        private IWidget unitInfoWidget;

        public ExploreMapState(IUnit unit)
        {
            this.unit = unit;
            CheckUnitInfoDisplay();
        }

        public override void Disable()
        {
            if (unitInfoWidget != null)
            {
                unitInfoWidget.Dispose();
            }
        }

        public override void Enable()
        {
        }

        public override void HandleAccept()
        {
            GameManager.instance.GameState = new ActiveUnitMenuState(unit);
        }

        public override void HandleCancel()
        {

        }


        public override void HandleCursorMove()
        {
            CheckUnitInfoDisplay();
        }

        protected void CheckUnitInfoDisplay()
        {
            var selectedTile = GameManager.instance.levelManager.GetMap().GetSelectedTile();

            // Create the unit info box
            if (selectedTile.HasUnit())
            {
                if (unitInfoWidget != null)
                {
                    unitInfoWidget.Dispose();
                    unitInfoWidget = null;
                }

                unitInfoWidget = new UnitInfoWidget(selectedTile.GetUnit());
            }
            else if (unitInfoWidget != null)
            {
                unitInfoWidget.Dispose();
                unitInfoWidget = null;
            }
        }
    }
}
