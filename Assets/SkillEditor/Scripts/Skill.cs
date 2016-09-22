using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.SkillEditor.Scripts;
using Rpg.Unit;
using UnityEngine;

namespace SkillEditor
{
    public class Skill : MonoBehaviour
    {
        // Testing attributes
        public GameObject source;
        public GameObject target;
        public bool isTest;

        // TODO Add checkbox to enable testing

        public List<ObjectTrigger> objectTriggers = new List<ObjectTrigger>();

        private SkillRenderer skillRenderer;

        public void Awake()
        {
        }

        public void Start()
        {
            // If test, then run the skill with the test params.
            if (isTest)
            {
                var skillSource = Instantiate(source);
                skillSource.transform.position = new Vector3(0, 0, 0);
                skillSource.transform.parent = gameObject.transform;

                var skillTarget = Instantiate(target);
                skillTarget.transform.position = new Vector3(1, 0, 0);
                skillTarget.transform.parent = gameObject.transform;

                var sourceUnit = skillSource.GetComponent<IUnit>();
                var targetUnits = new List<IUnit>();
                targetUnits.Add(skillTarget.GetComponent<IUnit>());
                var skillTargetObject = new SkillTarget(target.transform.position, targetUnits);

                Cast(sourceUnit, skillTargetObject);
            }
        }

        public void Cast(IUnit sourceUnit, ISkillTarget skillTarget)
        {
            skillRenderer = GetComponent<SkillRenderer>();

            skillRenderer.skill = this;
            skillRenderer.skillTarget = skillTarget;
            skillRenderer.sourceUnit = sourceUnit;

            var animator = GetComponent<Animator>();
            animator.SetTrigger("StartCast");
           
        }
    }
}
