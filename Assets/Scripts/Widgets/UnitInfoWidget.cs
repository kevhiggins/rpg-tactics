using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg.Widgets
{
    public class UnitInfoWidget : AbstractWidget
    {
        private IUnit unit;
        private GameObject unitInfoBox;

        public UnitInfoWidget(IUnit unit)
        {
            unitInfoBox = UnityEngine.Object.Instantiate(GameManager.instance.unitInfoBox);
            PopulateUnitInfoDisplay(unit);
        }

        public override void Dispose()
        {
            UnityEngine.Object.Destroy(unitInfoBox);
        }

        protected void PopulateUnitInfoDisplay(IUnit targetUnit)
        {
            var panel = GameObjectHelper.FindChildByName(unitInfoBox, "Panel");
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
