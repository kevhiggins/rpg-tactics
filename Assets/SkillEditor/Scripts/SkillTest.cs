using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkillEditor
{
    public class SkillTest : MonoBehaviour
    {
        public Skill skill;

        public GameObject skillSource;
        public GameObject skillTarget;

        private List<GameObject> triggeredObjects;

        public void Awake()
        {
            triggeredObjects = new List<GameObject>();

            skill.OnTriggerObject += TriggerObject;
            skill.OnEndSkill += EndSkill;

            skillSource = Instantiate(skill.source);
            skillSource.transform.position = new Vector3(0, 0, 0);
            skillSource.transform.parent = gameObject.transform;

            skillTarget = Instantiate(skill.target);
            skillTarget.transform.position = new Vector3(1, 0, 0);
            skillTarget.transform.parent = gameObject.transform;
        }

        public void OnDestroy()
        {
            skill.OnTriggerObject -= TriggerObject;
            skill.OnEndSkill -= EndSkill;
        }

        protected void TriggerObject(string triggerName)
        {
            var triggerObject = FindTriggerObject(triggerName);
            if(triggerObject == null)
                throw new Exception("Could not find trigger object with name " + triggerName);

            var skillEffect = Instantiate(triggerObject.triggerableItem);
            triggeredObjects.Add(skillEffect);

            if (triggerObject.parent == TriggerParent.Source)
            {
                skillEffect.transform.position = skillSource.transform.position + skillEffect.transform.position;
                skillEffect.transform.parent = skillSource.transform;
            }
            else if (triggerObject.parent == TriggerParent.Destination)
            {
                skillEffect.transform.position = skillTarget.transform.position + skillEffect.transform.position;
                skillEffect.transform.parent = skillTarget.transform;
            }

            //
            //

            //
            //            var skillEffect = Instantiate(skill.effect);
            //            skillEffect.transform.position = skillTarget.transform.position + skillEffect.transform.position;
            //            skillEffect.transform.parent = skillTarget.transform;
            //            
            //            var selections = new GameObject[1];
            //            selections[0] = skillEffect;
            //            Selection.objects = selections;
            //
            //            var skillAnimator = skill.GetComponent<Animator>();
            //            skillAnimator.Play("BlizzardAnimation");

        }

        protected void EndSkill()
        {
            foreach (var triggeredObject in triggeredObjects)
            {
                Destroy(triggeredObject);
            }

            triggeredObjects.Clear();
        }

        protected ObjectTrigger FindTriggerObject(string name)
        {
            foreach (var objectTrigger in skill.objectTriggers)
            {
                if (objectTrigger.name == name)
                    return objectTrigger;
            }
            return null;
        }
    }
}
