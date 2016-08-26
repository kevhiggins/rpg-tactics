using System;
using UnityEditor;
using UnityEngine;

namespace TileMapEditor.Editor
{
    [CustomEditor(typeof(TileMap))]
    class TileMapInspector : UnityEditor.Editor
    {
        public TileMap map;
        public TileBrush brush;
        private Vector3 mouseHitPosition;

        private bool MouseOnMap
        {
            // TODO revamp this to be relative to the map object position
            get { return mouseHitPosition.x > 0 && mouseHitPosition.x < map.gridSize.x && mouseHitPosition.y < 0 && mouseHitPosition.y > -map.gridSize.y ; }
        }

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

                if (GUILayout.Button("Clear Tiles"))
                {
                    if(EditorUtility.DisplayDialog("Clear map's tiles?", "Are you sure?", "Clear", "Do not clear"))
                    {
                        ClearMap();
                    }
                    ;
                }
            }

            EditorGUILayout.EndVertical();
        }


        void OnEnable()
        {
            map = target as TileMap;
            Tools.current = Tool.View;

            if (map.tiles == null)
            {
                var gameObject = new GameObject("Tiles");
                gameObject.transform.SetParent(map.transform);
                gameObject.transform.position = Vector3.zero;
                map.tiles = gameObject;
            }

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

        void OnSceneGUI()
        {
            if (brush != null)
            {
                UpdateHitPosition();
                MoveBrush();

                if (map.texture2D != null && MouseOnMap)
                {
                    var current = Event.current;
                    if (current.shift)
                    {
                        Draw();
                    } else if (current.alt)
                    {
                        RemoveTile();
                    }
                }
            }
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

        void UpdateHitPosition()
        {
            var p = new Plane(map.transform.TransformDirection(Vector3.forward), Vector3.zero);
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            var hit = Vector3.zero;
            var dist = 0f;

            if (p.Raycast(ray, out dist))
                hit = ray.origin + ray.direction.normalized*dist;

            mouseHitPosition = map.transform.InverseTransformPoint(hit);
        }

        void MoveBrush()
        {
            var tileSize = map.tileSize.x/map.pixelsToUnits;
            var x = Mathf.Floor(mouseHitPosition.x/tileSize)*tileSize;
            var y = Mathf.Floor(mouseHitPosition.y/tileSize)*tileSize;

            var row = x/tileSize;
            var column = Mathf.Abs(y/tileSize) - 1;

            if (!MouseOnMap)
                return; 
            var id = (column*map.mapSize.x + row);
            brush.tileID = Convert.ToInt32(id);

            x += map.transform.position.x + tileSize/2;
            y += map.transform.position.y + tileSize/2;

            brush.transform.position = new Vector3(x, y, map.transform.position.z);
        }

        void Draw()
        {
            var id = brush.tileID.ToString();
            var posX = brush.transform.position.x;
            var posY = brush.transform.position.y;

            GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);
            if (tile == null)
            {
                tile = new GameObject("tile_" + id);
                tile.transform.SetParent(map.tiles.transform);
                tile.transform.position = new Vector3(posX, posY, 0);
                tile.AddComponent<SpriteRenderer>();
            }

            tile.GetComponent<SpriteRenderer>().sprite = brush.renderer2D.sprite; 
        }

        void RemoveTile()
        {
            var id = brush.tileID.ToString();
            GameObject tile = GameObject.Find(map.name + "/Tiles/tile_" + id);

            if (tile != null)
            {
                DestroyImmediate(tile);
            }
        }

        void ClearMap()
        {

            for(var i=0; i < map.tiles.transform.childCount; i++)
            {
                Transform t = map.tiles.transform.GetChild(i);
                DestroyImmediate(t.gameObject);
                i--;
            }
        }
    }
}