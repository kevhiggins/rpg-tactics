using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rpg.Unit
{
    /// <summary>
    /// ActionQueue is used to keep track of which unit's turn it is. It handles calling each IUnit.ClockTick() method, sorting the list of units, and returning the
    /// active unit from the front of the list.
    /// </summary>
    class ActionQueue : MonoBehaviour, IActionQueue
    {
        public int CTThreshold = 100;

        private List<IUnit> unitList = new List<IUnit>();
        private IComparer<IUnit> unitComparer = new SortUnitsByCT();

        public List<IUnit> UnitList
        {
            get
            {
                return unitList;
            }
        }

        public int ChargeTimeThreshold
        {
            get
            {
                return CTThreshold;
            }
        }
        /// <summary>
        /// Return the first unit on the list if it has enough CT. Otherwise, return null.
        /// The return value will only change after the ClockTick method is called.
        /// </summary>
        /// <returns></returns>
        public IUnit GetActiveUnit()
        {
            // Sort the unit list to determine which unit is currently next in line.
            unitList.Sort(unitComparer);

            var nextUnit = unitList.FirstOrDefault();

            if (nextUnit == null)
            {
                return null;
            }

            return nextUnit.ChargeTime >= CTThreshold ? nextUnit : null;
        }

        public void ClockTick()
        {
            // Progress time for each unit in the unit list.
            foreach(IUnit unit in unitList)
            {
                unit.ClockTick();
            }
        }

        public List<IUnit> GetEnemyUnits(int friendlyTeamId)
        {
            var enemyUnits = new List<IUnit>();

            foreach (var unit in unitList)
            {
                if (unit.TeamId != friendlyTeamId)
                {
                    enemyUnits.Add(unit);
                }
            }
            return enemyUnits;
        }
    }
}
