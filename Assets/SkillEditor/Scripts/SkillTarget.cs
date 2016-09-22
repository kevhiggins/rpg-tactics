using System.Collections.Generic;
using Rpg.Unit;
using UnityEngine;

namespace SkillEditor
{
    public class SkillTarget : ISkillTarget
    {
        public Vector3 Position { get; private set; }
        public List<IUnit> Units { get; private set; }

        public SkillTarget(Vector3 position, List<IUnit> units)
        {
            Position = position;
            Units = units;
        }
    }
}
