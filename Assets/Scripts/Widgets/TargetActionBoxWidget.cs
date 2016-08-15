using System;
using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rpg.Widgets
{
    class TargetActionBoxWidget : UiWidget
    {
        private IUnit sourceUnit;
        private IUnit targetUnit;

        public TargetActionBoxWidget(IUnit sourceUnit, IUnit targetUnit) : base(CreateCanvas())
        {
            this.sourceUnit = sourceUnit;
            this.targetUnit = targetUnit;

            var sourceUnitPanel = GameObjectHelper.FindChildByName(canvas, "SourceUnitInfo");
            var targetUnitPanel = GameObjectHelper.FindChildByName(canvas, "TargetUnitInfo");
            var actionInfo = GameObjectHelper.FindChildByName(canvas, "ActionInfo");

            // Populate TargetActionBox data
            UnitInfoWidget.PopulateUnitInfoDisplay(sourceUnitPanel, sourceUnit);
            UnitInfoWidget.PopulateUnitInfoDisplay(targetUnitPanel, targetUnit);
        }

        private static GameObject CreateCanvas()
        {
            return Object.Instantiate(GameManager.instance.targetActionBox);
        }


        public override void HandleInput()
        {
        }
    }
}
