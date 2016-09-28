using System;
using System.Linq;
using System.Collections.Generic;
using Rpg.Unit;
using SkillEditor;
using UnityEngine;

namespace Assets.SkillEditor.Scripts
{
    public class SkillRenderer : MonoBehaviour
    {
        [HideInInspector] public Skill skill;
        [HideInInspector] public IUnit sourceUnit;
        [HideInInspector] public ISkillTarget skillTarget;

        public delegate void AfterEndSkillHandler();

        public event AfterEndSkillHandler OnAfterEndSkill = () => { };

        private Dictionary<string, List<GameObject>> triggeredObjects = new Dictionary<string, List<GameObject>>();
        private int hitCount = 0;

        public void OnDestroy()
        {
            // TODO Destroy all trigger objects
        }

        public void TriggerObject(string triggerName)
        {
            var triggerObject = FindTriggerObject(triggerName);
            if (triggerObject == null)
                throw new Exception("Could not find trigger object with name " + triggerName);

            var prefab = triggerObject.triggerableItem;


            if (triggerObject.parent == TriggerParent.Source)
            {
                CreateTriggerObject(triggerName, prefab, sourceUnit.GetGameObject().transform.position,
                    sourceUnit.GetGameObject());
            }
            else if (triggerObject.parent == TriggerParent.Destination)
            {
                foreach (var targetUnit in skillTarget.Units)
                {
                    CreateTriggerObject(triggerName, prefab, targetUnit.GetGameObject().transform.position,
                        targetUnit.GetGameObject());
                }
            }
        }

        public void DestroyObject(string triggerName)
        {
            if (triggeredObjects.ContainsKey(triggerName))
            {
                foreach (var item in triggeredObjects[triggerName])
                {
                    Destroy(item);
                }
                triggeredObjects[triggerName].Clear();
                triggeredObjects.Remove(triggerName);
            }
        }

        public void TriggerSourceAnimator(string triggerName)
        {
            sourceUnit.GetAnimator().SetTrigger(triggerName);
        }

        public void TriggerTargetAnimators(string triggerName)
        {
            foreach (var unit in skillTarget.Units)
            {
                unit.GetAnimator().SetTrigger(triggerName);
            }
        }

        public void EndSkill()
        {
            if (hitCount != skill.hits.Count)
                throw new Exception(
                    "Number of performed hits does not match the number of hits specified for this skill.");

            foreach (KeyValuePair<string, List<GameObject>> entry in triggeredObjects)
            {
                foreach (var item in entry.Value)
                {
                    Destroy(item);
                }
                entry.Value.Clear();
            }
            triggeredObjects.Clear();
            hitCount = 0;
            OnAfterEndSkill();
        }

        public void Hit()
        {
            var hit = skill.hits.ElementAtOrDefault(hitCount);
            if (hit == null)
            {
                throw new Exception("No hit exists at index " + hitCount);
            }

            hitCount++;


            // Find hit at index
            // Trigger Hit
        }

        protected void TriggerHit(Hit hit)
        {
            foreach (var unit in skillTarget.Units)
            {
//                unit
            }
            
        }

        protected void CreateTriggerObject(string triggerName, GameObject prefab, Vector3 parentPosition,
            GameObject parent)
        {
            var triggerObject = Instantiate(prefab);

            if (!triggeredObjects.ContainsKey(triggerName))
            {
                triggeredObjects[triggerName] = new List<GameObject>();
            }
            triggeredObjects[triggerName].Add(triggerObject);

            // Offset the position of the GameObject based on the parentPosition
            triggerObject.transform.position = parentPosition + triggerObject.transform.position;
            if (parent != null)
            {
                triggerObject.transform.parent = parent.transform;
            }
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