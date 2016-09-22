using System.Collections.Generic;
using Rpg.Unit;
using UnityEngine;

namespace SkillEditor
{
    public interface ISkillTarget
    {
        /// <summary>
        /// The position of the targeted area.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// The units impacted by the skill.
        /// </summary>
        List<IUnit> Units { get; }
    }
}
