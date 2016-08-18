using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg.Widgets
{
    public class UnitInfoWidget : UiWidget
    {
        private IUnit unit;

        public UnitInfoWidget(IUnit unit) : base(CreateCanvas())
        {
            var panel = GameObjectHelper.FindChildByName(canvas, "Panel");
            PopulateUnitInfoDisplay(panel, unit);
        }

        private static GameObject CreateCanvas()
        {
            return Object.Instantiate(GameManager.instance.unitInfoBox);
        }

        public override void HandleInput()
        {
        }

        public static void PopulateUnitInfoDisplay(GameObject panel, IUnit targetUnit)
        {
            ReplaceTextData(panel, "hitpoints", "{hp / hpmax}", targetUnit.CurrentHp + " / " + targetUnit.MaxHp);
            ReplaceTextData(panel, "name", "{unitname}", targetUnit.UnitName);
            ReplaceTextData(panel, "chargetime", "{ct / max}",
                targetUnit.ChargeTime + " / " + GameManager.instance.actionQueue.ChargeTimeThreshold);
            ReplaceTextData(panel, "level", "{level}", targetUnit.Level.ToString());
            ReplaceTextData(panel, "speed", "{speed}", targetUnit.Speed.ToString());
            ReplaceTextData(panel, "movespeed", "{ms}", targetUnit.MovementSpeed.ToString());
            ReplaceTextData(panel, "expamount", "{exp}", targetUnit.Experience.ToString());
            ReplaceTextData(panel, "expamount", "{nextlevel}", targetUnit.ExperienceToLevel.ToString());

            SetBarValue(panel, "hpbar", "hpbar", (float) targetUnit.CurrentHp/targetUnit.MaxHp);
            SetBarValue(panel, "ctbar", "ctbar", (float)targetUnit.ChargeTime / GameManager.instance.actionQueue.ChargeTimeThreshold);
            SetBarValue(panel, "expbar", "expbar", (float)targetUnit.Experience / targetUnit.ExperienceToLevel);

            var spriteRenderer = targetUnit.GetSpriteRenderer();
            var targetSprite = spriteRenderer.sprite;

            var unitPortraitObject = GameObjectHelper.FindChildByName(panel, "UnitPortrait");
            var imageScript = unitPortraitObject.GetComponent<Image>();
            imageScript.sprite = targetSprite;
            imageScript.color = spriteRenderer.color;
        }

        protected static void SetBarValue(GameObject panel, string barName, string currentBarName, float value)
        {
            var bars = GameObjectHelper.FindChildByName(panel, "Bars");
            var parentBar = GameObjectHelper.FindChildByName(bars, barName);

            var childBar = GameObjectHelper.FindChildByName(parentBar, currentBarName);
            var barImageScript = childBar.GetComponent<Image>();
            barImageScript.fillAmount = value;
        }

        protected static void ReplaceTextData(GameObject parent, string textFieldName, string targetData, string newData)
        {
            var hitPointObject = GameObjectHelper.FindChildByName(parent, textFieldName);
            var textScript = hitPointObject.GetComponent<Text>();

            textScript.text = textScript.text.Replace(targetData, newData);
        }
    }
}