﻿using Assets.Scripts.Unity;
using Rpg.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameState
{
    class ExploreMapState : AbstractMapGameState
    {
        private IUnit unit;
        private GameObject unitInfoBox;

        public ExploreMapState(IUnit unit)
        {
            this.unit = unit;
            CheckUnitInfoDisplay();
        }

        public override void Disable()
        {
            UnityEngine.Object.Destroy(unitInfoBox);
        }

        public override void Enable()
        {
        }

        public override void HandleAccept()
        {
            // Get current tile
            var selectedTile = GameManager.instance.levelManager.GetMap().GetSelectedTile();
            if(selectedTile.HasUnit())
            {
                if(selectedTile.GetUnit().Equals(unit))
                {
                    GameManager.instance.GameState = new ActiveUnitMenuState(unit);
                }
            }
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
                if (unitInfoBox != null)
                {
                    UnityEngine.Object.Destroy(unitInfoBox);
                }
                unitInfoBox = UnityEngine.Object.Instantiate(GameManager.instance.unitInfoBox);
                PopulateUnitInfoDisplay(selectedTile.GetUnit());
            }
            else if (unitInfoBox != null)
            {
                UnityEngine.Object.Destroy(unitInfoBox);
            }
        }

        protected void PopulateUnitInfoDisplay(IUnit targetUnit)
        {
            var panel = GameObjectHelper.FindChildByName(unitInfoBox, "Panel");
            ReplaceTextData(panel, "hitpoints", "{hp / hpmax}", targetUnit.CurrentHp + " / " + targetUnit.MaxHp);
            ReplaceTextData(panel, "name", "{unitname}", targetUnit.UnitName);
            ReplaceTextData(panel, "chargetime", "{ct / max}", targetUnit.ChargeTime + " / " + GameManager.instance.actionQueue.ChargeTimeThreshold);
            ReplaceTextData(panel, "level", "{level}", targetUnit.Level.ToString());
            ReplaceTextData(panel, "speed", "{speed}", targetUnit.Speed.ToString());
            ReplaceTextData(panel, "movespeed", "{movespeed}", targetUnit.MovementSpeed.ToString());
        }

        protected void ReplaceTextData(GameObject parent, string textFieldName, string targetData, string newData)
        {
            var hitPointObject = GameObjectHelper.FindChildByName(parent, textFieldName);
            var textScript = hitPointObject.GetComponent<Text>();

            textScript.text = textScript.text.Replace(targetData, newData);
        }
    }
}
