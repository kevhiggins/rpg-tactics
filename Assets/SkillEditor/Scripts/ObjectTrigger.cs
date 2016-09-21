using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkillEditor
{
    public enum TriggerParent
    {
        Source,
        Destination,
        World
    }

    [System.Serializable]
    public class ObjectTrigger
    {
        public string name;
        public GameObject triggerableItem;
        public TriggerParent parent;
    }
}
