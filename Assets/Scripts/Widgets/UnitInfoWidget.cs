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
            PopulateUnitInfoDisplay(unit);

            var canvasScript = canvas.GetComponent<Canvas>();
            canvasScript.worldCamera = Camera.main;
        }

        private static GameObject CreateCanvas()
        {
            return Object.Instantiate(GameManager.instance.unitInfoBox);
        }

        public override void Dispose()
        {
            Object.Destroy(canvas);
        }

        public override void HandleInput()
        {
        }

        protected void PopulateUnitInfoDisplay(IUnit targetUnit)
        {
            var panel = GameObjectHelper.FindChildByName(canvas, "Panel");
            ReplaceTextData(panel, "hitpoints", "{hp / hpmax}", targetUnit.CurrentHp + " / " + targetUnit.MaxHp);
            ReplaceTextData(panel, "name", "{unitname}", targetUnit.UnitName);
            ReplaceTextData(panel, "chargetime", "{ct / max}", targetUnit.ChargeTime + " / " + GameManager.instance.actionQueue.ChargeTimeThreshold);
            ReplaceTextData(panel, "level", "{level}", targetUnit.Level.ToString());
            ReplaceTextData(panel, "speed", "{speed}", targetUnit.Speed.ToString());
            ReplaceTextData(panel, "movespeed", "{ms}", targetUnit.MovementSpeed.ToString());

            var spriteRenderer = targetUnit.GetGameObject().transform.GetChild(0).GetComponent<SpriteRenderer>();
            var targetSprite = spriteRenderer.sprite;

            var unitPortraitObject = GameObjectHelper.FindChildByName(panel, "UnitPortrait");
            var imageScript = unitPortraitObject.GetComponent<Image>();
            imageScript.sprite = targetSprite;
            imageScript.color = spriteRenderer.color;
        }

        protected void ReplaceTextData(GameObject parent, string textFieldName, string targetData, string newData)
        {
            var hitPointObject = GameObjectHelper.FindChildByName(parent, textFieldName);
            var textScript = hitPointObject.GetComponent<Text>();

            textScript.text = textScript.text.Replace(targetData, newData);
        }
    }
}
