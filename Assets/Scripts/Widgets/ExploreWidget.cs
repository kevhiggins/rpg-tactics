namespace Rpg.Widgets
{
    class ExploreWidget : AbstractWidget
    {
        private UnitInfoWidget unitInfoWidget;
        private MapCursorWidget mapCursorWidget;

        public ExploreWidget()
        {
            UnitInfoWidget.CheckUnitInfoDisplay(ref unitInfoWidget);
            mapCursorWidget = new MapCursorWidget();
            mapCursorWidget.OnCursorMove += () =>
            {
                UnitInfoWidget.CheckUnitInfoDisplay(ref unitInfoWidget);
            };
        }

        public override void Dispose()
        {
            if (unitInfoWidget != null)
            {
                unitInfoWidget.Dispose();
            }
            mapCursorWidget.Dispose();
        }

        public override void HandleInput()
        {
            mapCursorWidget.HandleInput();
        }
    }
}
