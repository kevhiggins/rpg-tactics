using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileMapEditor;
using UnityEditor;
using UnityEngine;

namespace SkillEditor.Editor
{
    class NewSkillMenu
    {
        [MenuItem("GameObject/Skill")]
        public static void CreateTileMap()
        {
            GameObject gameObject = new GameObject("Skill");
            gameObject.AddComponent<Skill>();
        }
    }
}
