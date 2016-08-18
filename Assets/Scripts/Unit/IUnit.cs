using UnityEngine;
using Rpg.Map;

namespace Rpg.Unit
{
    public delegate void LevelUpHandler(IUnit unit, int level);

    public delegate void ExperienceGainHandler(IUnit unit, int amount);

    public delegate void DamageHandler(IUnit unit, int amount);

    public delegate void AttackHitHandler();

    public delegate void AttackCompleteHandler();

    public delegate void DeathCompleteHandler();

    public interface IUnit : ITileChild
    {
        int MovementSpeed { get; }
        int Speed { get; }
        int ChargeTime { get; }
        int CurrentHp { get; }
        int MaxHp { get; }
        string UnitName { get; }
        int Level { get; }
        int Damage { get; }
        int Experience { get; }
        int ExperienceWorth { get; }
        int ExperienceToLevel { get; }
        bool IsDead { get; }

        // TODO This should be temporary
        TilePosition StartPosition { get; }

        GameObject GetGameObject();
        SpriteRenderer GetSpriteRenderer();

        /// <summary>
        /// Perform any operations necessary for a new clock tick.
        /// Examples: Increase charge time, check on charged abilities? Heal overtime from a buff/debuff etc.
        /// </summary>
        void ClockTick();

        /// <summary>
        /// Called at the beginning of the unit's turn.
        /// </summary>
        void StartTurn();

        /// <summary>
        /// Called at the end of the unit's turn.
        /// </summary>
        void EndTurn();

        void Attack(TilePosition targetPosition);

        void TakeDamage(int damage);

        void GainExperience(int amount);

        event LevelUpHandler OnLevelUp;
        event ExperienceGainHandler OnExperienceGain;
        event DamageHandler OnDamage;

        /// <summary>
        /// When the current unit's attack contacts its target.
        /// </summary>
        event AttackHitHandler OnAttackHit;

        event AttackCompleteHandler OnAttackComplete;
        event DeathCompleteHandler OnDeathComplete;
    }
}