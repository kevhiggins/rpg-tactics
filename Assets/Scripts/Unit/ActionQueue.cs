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
            var nextUnit = unitList.First();

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

            // Sort the unit list to determine which unit is currently next in line.
            unitList.Sort(unitComparer);
        }

        public List<IFriendlyUnit> GetFriendlyUnits()
        {
            var friendlyUnits = new List<IFriendlyUnit>();

            foreach (var unit in unitList)
            {
                if (unit is IFriendlyUnit)
                {
                    friendlyUnits.Add((IFriendlyUnit)unit);
                }
            }
            return friendlyUnits;
        }
    }
}
