using UnityEditor;
using UnityEngine;

namespace TileMapEditor.Editor
{
    [CustomEditor(typeof(TileMap))]
    class TileMapInspector : UnityEditor.Editor
    {
        public TileMap map;
        public TileBrush brush;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();

            var oldSize = map.mapSize;
            map.mapSize = EditorGUILayout.Vector2Field("Map Size:", map.mapSize);
            if (map.mapSize != oldSize)
            {
                UpdateCalculations();
            }

            map.texture2D =
                (Texture2D) EditorGUILayout.ObjectField("Texture2D:", map.texture2D, typeof(Texture2D), false);

            if (map.texture2D == null)
            {
                EditorGUILayout.HelpBox("You have not selected a texture 2d yet.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.LabelField("Tile Size:", map.tileSize.x + "x" + map.tileSize.y);
                EditorGUILayout.LabelField("Grid Size In Units:", map.gridSize.x + "x" + map.gridSize.y);
                EditorGUILayout.LabelField("Pixels To Units:", map.pixelsToUnits.ToString());
                UpdateBrush(map.CurrentTileBrush);
            }

            EditorGUILayout.EndVertical();
        }


        void OnEnable()
        {
            map = target as TileMap;
            Tools.current = Tool.View;
            UpdateCalculations();

            if (map.texture2D != null)
            {
                NewBrush();
            }
        }

        void OnDisable()
        {
            DestroyBrush();
        }

        void UpdateCalculations()
        {
            if (map.texture2D == null)
                return;

            var path = AssetDatabase.GetAssetPath(map.texture2D);
            map.spriteReferences = AssetDatabase.LoadAllAssetsAtPath(path);

            // First item is the Texture2D itself. Second item is the first sliced sprite.
            var sprite = (Sprite) map.spriteReferences[1];

            var width = sprite.textureRect.width;
            var height = sprite.textureRect.height;

            map.pixelsToUnits = sprite.rect.width/sprite.bounds.size.x;

            map.tileSize = new Vector2(width, height);

            map.gridSize = new Vector2(width/map.pixelsToUnits*map.mapSize.x, height/map.pixelsToUnits*map.mapSize.y);
        }

        void CreateBrush()
        {
            var sprite = map.CurrentTileBrush;
            if (sprite != null)
            {
                GameObject gameObject = new GameObject("Brush");
                gameObject.transform.SetParent(map.transform);

                brush = gameObject.AddComponent<TileBrush>();
                brush.renderer2D = gameObject.AddComponent<SpriteRenderer>();
                var pixelsToUnits = map.pixelsToUnits;

                // TODO move this calculation into UpdateBrush
                brush.brushSize = new Vector2(sprite.textureRect.width/pixelsToUnits,
                    sprite.textureRect.height/pixelsToUnits);
                brush.UpdateBrush(sprite);
            }
        }

        void NewBrush()
        {
            if (brush == null)
                CreateBrush();
        }

        void DestroyBrush()
        {
            if (brush != null)
                DestroyImmediate(brush.gameObject);
        }

        public void UpdateBrush(Sprite sprite)
        {
            if (brush != null)
            {
                brush.UpdateBrush(sprite);
            }
        }
    }
}