using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rpg.Unit;
using UnityEngine;

namespace SkillEditor
{
    public class Skill : MonoBehaviour
    {
        // Testing attributes
        public GameObject source;
        public GameObject target;

        public List<ObjectTrigger> objectTriggers = new List<ObjectTrigger>();

        public delegate void TriggerObjectHandler(string triggerName);
        public event TriggerObjectHandler OnTriggerObject = triggerName => { };

        public delegate void EndSkillHanlder();
        public event EndSkillHanlder OnEndSkill = () => { };

        public void TriggerObject(string triggerName)
        {
            OnTriggerObject(triggerName);

            // Make this into an event
            // Inspector hooks into event
            // On event trigger, 

        }

        public void EndSkill()
        {
            OnEndSkill();
        }
    }
}
