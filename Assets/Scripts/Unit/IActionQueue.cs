using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpg.Unit
{
    public interface IActionQueue
    {
        List<IUnit> UnitList { get; }

        /// <summary>
        /// Return the IUnit whose turn it is now. Returns null, if it is no unit's turn yet.
        /// </summary>
        /// <returns></returns>
        IUnit GetActiveUnit();

        /// <summary>
        /// Marks the passage of time. Useful for actions that take time, or charging how long till a unit's turn happens.
        /// </summary>
        void ClockTick();
    }
}
