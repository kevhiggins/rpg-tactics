using Rpg.Unit;
using UnityEngine;

namespace Rpg
{
    public class BattleManager : MonoBehaviour
    {
        public void AttackUnit(IUnit sourceUnit, IUnit targetUnit)
        {
            sourceUnit.Attack();
            targetUnit.TakeDamage(sourceUnit.Damage);
            sourceUnit.EndTurn();
        }
    }
}
