using System;
using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Rpg.Widgets
{
    class TargetActionBoxWidget : UiWidget
    {
        public TargetActionBoxWidget(IUnit sourceUnit, IUnit targetUnit) : base(CreateCanvas())
        {
            var sourceUnitPanel = GameObjectHelper.FindChildByName(canvas, "SourceUnitInfo");
            var targetUnitPanel = GameObjectHelper.FindChildByName(canvas, "TargetUnitInfo");
            var actionInfo = GameObjectHelper.FindChildByName(canvas, "ActionInfo");

            ReplaceTextData(actionInfo, "DamageValue", "{dmg}", sourceUnit.Damage.ToString());

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

        // TODO Centralize this
        protected static void ReplaceTextData(GameObject parent, string textFieldName, string targetData, string newData)
        {
            var hitPointObject = GameObjectHelper.FindChildByName(parent, textFieldName);
            var textScript = hitPointObject.GetComponent<Text>();

            textScript.text = textScript.text.Replace(targetData, newData);
        }
    }
}
