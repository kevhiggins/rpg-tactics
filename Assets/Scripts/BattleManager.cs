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

            DeathCompleteHandler deathCompleteHandler = null;
            targetUnit.OnDeathComplete += deathCompleteHandler = () =>
            {
                // Unregister callback.
                targetUnit.OnDeathComplete -= deathCompleteHandler;

                // If the target unit is dead, and they were worth experience, grant it to the attacking unit.
                if (targetUnit.IsDead && targetUnit.ExperienceWorth > 0)
                {
                    sourceUnit.GainExperience(targetUnit.ExperienceWorth);
                }
                sourceUnit.EndTurn();
            };

            // When the hit lands, apply damage to the target unit.
            AttackHitHandler attackHitHandler = null;
            sourceUnit.OnAttackHit += attackHitHandler = () =>
            {
                // Unregister the callback
                sourceUnit.OnAttackHit -= attackHitHandler;

                targetUnit.TakeDamage(sourceUnit.Damage);
            };

            // When the attack is complete, check if the enemy is dead, award experience if necessary, and end the turn.
            AttackCompleteHandler attackCompleteHandler = null;
            sourceUnit.OnAttackComplete += attackCompleteHandler = () =>
            {
                // Unregister the callback
                sourceUnit.OnAttackComplete -= attackCompleteHandler;

                // If the target unit is not dead, then end the turn. Otherwise, the death handler will take care of this.
                if (targetUnit.IsDead == false)
                {
                    sourceUnit.EndTurn();
                }
            };

            // Ideally, if the enemy is dead, we should wait for the death animation to finish to end the turn.
            // Otherwise, just end the turn after the attack.

            sourceUnit.Attack(targetUnit.GetTile().tilePosition);
        }
    }
}
