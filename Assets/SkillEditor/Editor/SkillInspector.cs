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

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            skill.effect =
                (GameObject) EditorGUILayout.ObjectField("Effect", skill.effect, typeof(GameObject), false, null);
            skill.source =
                (GameObject) EditorGUILayout.ObjectField("Source", skill.source, typeof(GameObject), false, null);
            skill.target =
                (GameObject) EditorGUILayout.ObjectField("Target", skill.target, typeof(GameObject), false, null);

            if (GUILayout.Button("Test"))
            {
                PlayTest();
            }

            EditorGUILayout.EndVertical();


            //            serializedObject.Update();
            //            EditorGUILayout.BeginVertical();
            //
            //            var oldSize = map.mapSize;
            //            map.mapSize = EditorGUILayout.Vector2Field("Map Size:", map.mapSize);
            //            map.tileSize = EditorGUILayout.Vector2Field("Tile Size In Pixels", map.tileSize);
            //            map.pixelsToUnits = EditorGUILayout.IntField("Pixels To Units", map.pixelsToUnits);
            //
            //            map.impassableColor = EditorGUILayout.ColorField("Impassable Color:", map.impassableColor);
            //
            //            DisplayList("Penalty Colors:", "penaltyColors");
            //            DisplayList("Units:", "units");
            //
            //            if (map.mapSize != oldSize)
            //            {
            //                UpdateCalculations();
            //            }
            //
            //            EditorGUILayout.LabelField("Grid Size In Units:", map.gridSize.x + "x" + map.gridSize.y);
            //
            //            map.testMode = EditorGUILayout.Toggle("Test Mode:", map.testMode);
            //
            //            if (GUILayout.Button("Update Tiles"))
            //            {
            //                UpdateTiles();
            //            }
            //
            //            EditorGUILayout.EndVertical();
        }

        private void PlayTest()
        {
            var gameObject = new GameObject("Skill Test");
            var skillSource = Instantiate(skill.source);
            skillSource.transform.position = new Vector3(0, 0, 0);
            skillSource.transform.parent = gameObject.transform;

            var skillTarget = Instantiate(skill.target);
            skillTarget.transform.position = new Vector3(1, 0, 0);
            skillTarget.transform.parent = gameObject.transform;

            var skillEffect = Instantiate(skill.effect);
            skillEffect.transform.position = new Vector3(0, 0, 0);
            skillEffect.transform.parent = gameObject.transform;
        }
    }
}