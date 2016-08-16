using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg
{
    public class BattleManager : MonoBehaviour
    {
        public GameObject popCanvas;

        public void AttackUnit(IUnit sourceUnit, IUnit targetUnit)
        {
            sourceUnit.Attack(targetUnit.GetTile().tilePosition);
            targetUnit.TakeDamage(sourceUnit.Damage);
            DisplayDamagePop(targetUnit, sourceUnit.Damage);
            sourceUnit.EndTurn();
        }

        public void DisplayDamagePop(IUnit targetUnit, int damage)
        {
            var popObject = Instantiate(popCanvas);
            popObject.transform.position = targetUnit.GetGameObject().transform.position;
            var textObject = GameObjectHelper.FindChildByName(popObject, "FloatingDMG");
            var textScript = textObject.GetComponent<Text>();
            textScript.text = textScript.text.Replace("{dmg}", damage.ToString());
        }
    }
}
