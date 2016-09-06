using System.Collections.Generic;
using Rpg.Map;
using Rpg.PathFinding;

namespace Rpg.Unit
{
    public class UnitTurn
    {
        public IUnit ActiveUnit { get; private set; }
        public List<GraphNodeTile> MovementPath { get; set; }
        public Tile ActTargetTile { get; set; }

        public UnitTurn(IUnit activeUnit)
        {
            ActiveUnit = activeUnit;
            activeUnit.StartTurn();
        }
    }
}
