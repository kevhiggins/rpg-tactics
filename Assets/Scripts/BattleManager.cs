using System;
using System.Collections;
using Assets.Scripts.Unity;
using Rpg.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Rpg
{
    public class BattleManager : MonoBehaviour
    {
        public IUnit TargetUnit { get; set; }

        public void AttackUnit(IUnit sourceUnit, IUnit targetUnit, Action onComplete)
        {
            if (sourceUnit.DoSpellAttack)
            {
                var wizardCastEffect = Instantiate(GameManager.instance.wizardCastEffect,
                    sourceUnit.GetGameObject().transform.position, Quaternion.identity);
                Destroy(wizardCastEffect, 5);
                StartCoroutine(SpellHit(sourceUnit, targetUnit));
            }

            // When the hit lands, apply damage to the target unit.
            AttackHitHandler attackHitHandler = null;
            sourceUnit.OnAttackHit += attackHitHandler = () =>
            {
                // Unregister the callback
                sourceUnit.OnAttackHit -= attackHitHandler;

                targetUnit.TakeDamage(sourceUnit.Damage);

                // If the target is dead, then attach the deathCompleteHandler to complete the turn.
                if (targetUnit.IsDead)
                {
                    DeathCompleteHandler deathCompleteHandler = null;
                    targetUnit.OnDeathComplete += deathCompleteHandler = () =>
                    {
                        // Unregister callback.
                        targetUnit.OnDeathComplete -= deathCompleteHandler;

                        // If the target unit is dead, and they were worth experience, grant it to the attacking unit.
                        if (targetUnit.ExperienceWorth > 0)
                        {
                            sourceUnit.GainExperience(targetUnit.ExperienceWorth);
                        }
                        onComplete();
                    };
                }
                else
                {
                    // When the attack is complete, check if the enemy is dead, and award experience if necessary.
                    AttackCompleteHandler attackCompleteHandler = null;
                    sourceUnit.OnAttackComplete += attackCompleteHandler = () =>
                    {
                        // Unregister the callback
                        sourceUnit.OnAttackComplete -= attackCompleteHandler;

                        // If the target unit is not dead, then the attack is complete. Otherwise, the death handler will take care of this.
                        onComplete();
                    };
                }
            };


            // Ideally, if the enemy is dead, we should wait for the death animation to finish to end the turn.
            // Otherwise, just end the turn after the attack.

            sourceUnit.Attack(targetUnit.GetTile().tilePosition);
        }

        protected IEnumerator SpellHit(IUnit sourceUnit, IUnit targetUnit)
        {
            yield return new WaitForSeconds(sourceUnit.SpellDelay);
            var wizardHitEffect = Instantiate(GameManager.instance.wizardHitEffect,
                targetUnit.GetGameObject().transform.position, Quaternion.identity);
            Destroy(wizardHitEffect, 5);
        }
    }
}