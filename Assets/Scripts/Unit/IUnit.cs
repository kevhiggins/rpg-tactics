﻿using UnityEngine;
using Rpg.Map;

namespace Rpg.Unit
{
    public interface IUnit : ITileChild
    {
        int MovementSpeed { get; }
        int Speed { get; }
        int ChargeTime { get; }

        // TODO This should be temporary
        TilePosition StartPosition { get; }

        GameObject GetGameObject();

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
        /// Calleda t the end of the unit's turn.
        /// </summary>
        void EndTurn();
    }
}
