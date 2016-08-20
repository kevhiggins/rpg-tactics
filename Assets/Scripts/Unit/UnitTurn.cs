using System.Collections.Generic;
using Rpg.Map;
using UnityEngine;

namespace Rpg.Unit
{
    public class UnitTurn
    {
        public IUnit ActiveUnit { get; private set; }
        public List<Vector3> MovementPath { get; set; }
        public Tile ActTargetTile { get; set; }

        public UnitTurn(IUnit activeUnit)
        {
            ActiveUnit = activeUnit;
            activeUnit.StartTurn();
        }
    }
}
