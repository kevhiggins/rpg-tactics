using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg
{
    public class BattleManager : MonoBehaviour
    {
        public void AttackUnit(IUnit sourceUnit, IUnit targetUnit)
        {
            
            // When the hit lands, apply damage to the target unit.
            AttackHitHandler attackHitHandler = null;
            sourceUnit.OnAttackHit += attackHitHandler = () =>
            {
                sourceUnit.OnAttackHit -= attackHitHandler;
                targetUnit.TakeDamage(sourceUnit.Damage);
            };

            // When the attack is complete, check if the enemy is dead, award experience if necessary, and end the turn.
            AttackCompleteHandler attackCompleteHandler = null;
            sourceUnit.OnAttackComplete += attackCompleteHandler = () =>
            {
                if (targetUnit.IsDead && targetUnit.ExperienceWorth > 0)
                {
                    sourceUnit.GainExperience(targetUnit.ExperienceWorth);
                }
                sourceUnit.EndTurn();
            };

            sourceUnit.Attack(targetUnit.GetTile().tilePosition);
        }
    }
}
