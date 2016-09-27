using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace SkillEditor.Editor
{
    [CustomEditor(typeof(Skill))]
    class SkillInspector : UnityEditor.Editor
    {
        public Skill skill;

        void OnEnable()
        {
            skill = target as Skill;
        }

        protected void DisplayList(string label, string propertyName)
        {
            var objects = serializedObject.FindProperty(propertyName);

            EditorGUILayout.PropertyField(objects, new GUIContent(label), true);
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
//            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            skill.source =
                (GameObject) EditorGUILayout.ObjectField("Source", skill.source, typeof(GameObject), false, null);
            skill.target =
                (GameObject) EditorGUILayout.ObjectField("Target", skill.target, typeof(GameObject), false, null);

            skill.isTest = EditorGUILayout.Toggle("Enable Test Mode", skill.isTest);

            DisplayList("Object Triggers:", "objectTriggers");

            EditorGUILayout.EndVertical();

            // Save any changes to the Skill
            if (GUI.changed)
            {
                EditorUtility.SetDirty(skill);
            }
        }
    }
}