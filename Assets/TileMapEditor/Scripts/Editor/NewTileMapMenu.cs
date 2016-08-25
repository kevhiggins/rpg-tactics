using UnityEngine;
using UnityEditor;

namespace TileMapEditor.Editor
{
    public class NewTileMapMenu
    {

        [MenuItem("GameObject/Tile Map")]
        public static void CreateTileMap()
        {
            GameObject gameObject = new GameObject("Tile Map");
            gameObject.AddComponent<TileMap>();
        }
    }
}