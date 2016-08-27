using System;
using System.Collections.Generic;
using System.Linq;
using Assets.TileMapEditor.Scripts;
using UnityEditor;
using UnityEngine;

namespace TileMapEditor.Editor
{
    [CustomEditor(typeof(TileMap))]
    class TileMapInspector : UnityEditor.Editor
    {
        public TileMap map;

        private TileBrush brush;

        private Vector3 mouseHitPosition;
        private TilePickerWindow pickerWindow;

        private bool MouseOnMap
        {
            // TODO revamp this to be relative to the map object position
            get
            {
                return mouseHitPosition.x > 0 && mouseHitPosition.x < map.gridSize.x && mouseHitPosition.y < 0 &&
                       mouseHitPosition.y > -map.gridSize.y;
            }
        }

        protected void DisplayList(string label, string propertyName)
        {
            var penaltyColors = serializedObject.FindProperty(propertyName);

            EditorGUILayout.PropertyField(penaltyColors, new GUIContent(label), true);
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            var oldSize = map.mapSize;
            map.mapSize = EditorGUILayout.Vector2Field("Map Size:", map.mapSize);
            map.tileSize = EditorGUILayout.Vector2Field("Tile Size In Pixels", map.tileSize);
            map.pixelsToUnits = EditorGUILayout.IntField("Pixels To Units", map.pixelsToUnits);

            map.impassableColor = EditorGUILayout.ColorField("Impassable Color:", map.impassableColor);

            DisplayList("Penalty Colors:", "penaltyColors");
            DisplayList("Units:", "units");

            if (map.mapSize != oldSize)
            {
                UpdateCalculations();
            }

            EditorGUILayout.LabelField("Grid Size In Units:", map.gridSize.x + "x" + map.gridSize.y);

            map.testMode = EditorGUILayout.Toggle("Test Mode:", map.testMode);

            if (GUILayout.Button("Update Tiles"))
            {
                UpdateTiles();
            }

            EditorGUILayout.EndVertical();
        }

        private void UpdateTiles()
        {
            var penaltyColors = new Dictionary<int, PenaltyColor>();
            foreach (var penaltyColor in map.penaltyColors)
            {
                penaltyColors[penaltyColor.penalty] = penaltyColor;
            }

            for (var i = 0; i < map.tiles.transform.childCount; i++)
            {
                Transform t = map.tiles.transform.GetChild(i);
                var tile = t.gameObject.GetComponent<Tile>();
                if (tile.passable == false)
                {
                    tile.gameObject.GetComponent<SpriteRenderer>().sprite =
                        pickerWindow.GetFilledSprite((int) map.tileSize.x, (int) map.tileSize.y, map.impassableColor);
                }
                else
                {
                    if (penaltyColors.ContainsKey(tile.penalty))
                    {
                        tile.gameObject.GetComponent<SpriteRenderer>().sprite =
                            pickerWindow.GetFilledSprite((int) map.tileSize.x, (int) map.tileSize.y,
                                penaltyColors[tile.penalty].color);
                    }
                }
            }
            pickerWindow.UpdateSelection();
        }


        void OnEnable()
        {
            // Hook into window open event, to keep the window up to date.
            TilePickerWindow.OnWindowOpen += UpdateWindow;
            pickerWindow = TilePickerWindow.Instance;

            map = target as TileMap;
            Tools.current = Tool.View;

            if (map.tiles == null)
            {
                var gameObject = new GameObject("Tiles");
                gameObject.transform.SetParent(map.transform);
                gameObject.transform.position = Vector3.zero;
                map.tiles = gameObject;
            }

            InitializeTiles();
            UpdateCalculations();
            UpdateBrush(pickerWindow.SelectedSprite, pickerWindow.SelectedUnit);
        }

        private void InitializeTiles()
        {
            foreach (Transform tileTransform in map.tiles.transform)
            {
                var tile = tileTransform.GetComponent<Tile>();
                tile.Initialize();
            }
        }

        void OnDisable()
        {
            DestroyBrush();
            TilePickerWindow.OnWindowOpen -= UpdateWindow;
            if (pickerWindow != null)
                pickerWindow.OnSelectionChange -= UpdateBrush;
        }

        private void BeforeWindowDestroy(TilePickerWindow window)
        {
            DestroyBrush();
            pickerWindow.BeforeDestroy -= BeforeWindowDestroy;
            pickerWindow = null;
        }

        private void UpdateWindow(TilePickerWindow window)
        {
            if (pickerWindow == null)
            {
                window.OnSelectionChange += UpdateBrush;
                window.BeforeDestroy += BeforeWindowDestroy;
            }

            pickerWindow = window;
        }

        public void UpdateBrush(Sprite sprite, GameObject unit)
        {
            // If we have a sprite, and we don't have a brush, then create it.
            if (sprite != null || unit != null)
            {
                if (brush == null)
                {
                    GameObject gameObject = new GameObject("Brush");
                    gameObject.transform.SetParent(map.transform);
                    brush = gameObject.AddComponent<TileBrush>();
                    brush.renderer2D = gameObject.AddComponent<SpriteRenderer>();
                }
                brush.UpdateBrush(sprite, unit);
            }
            else
            {
                // Delete the brush, since there is no selection
                DestroyBrush();
            }
        }

        private void DestroyBrush()
        {
            if (brush != null)
            {
                DestroyImmediate(brush.gameObject);
                brush = null;
            }
        }


        void OnSceneGUI()
        {
            if (brush != null)
            {
                UpdateHitPosition();
                MoveBrush();

                if (MouseOnMap && (brush.renderer2D.sprite != null || brush.transform.childCount > 0))
                {
                    var current = Event.current;
                    if (current.shift)
                    {
                        Draw();
                    }
                    else if (current.alt)
                    {
                        RemoveTile();
                    }
                }
            }
        }

        void UpdateCalculations()
        {
            map.CalculateGridSize();

            var offset = map.gridSize/2;

            map.gameObject.transform.position = new Vector3(-offset.x, offset.y);
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
            brush.x = Convert.ToInt32(row);
            brush.y = Convert.ToInt32(column);

            x += map.transform.position.x + tileSize/2;
            y += map.transform.position.y + tileSize/2;

            // Pretty hacky way to keep the game object following the cursor. Find a better way.
            brush.transform.position = new Vector3(x, y, map.transform.position.z);
            if (brush.transform.childCount > 0)
            {
                var child = brush.transform.GetChild(0);
                child.position = brush.transform.position;
            }
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
                tile.AddComponent<Tile>();
            }

            var tileScript = tile.GetComponent<Tile>();
            tileScript.x = brush.x;
            tileScript.y = brush.y;

            pickerWindow.UpdateTile(tileScript);

            if (brush.renderer2D.sprite != null)
                tile.GetComponent<SpriteRenderer>().sprite = brush.renderer2D.sprite;

            if (brush.transform.childCount > 0)
            {
                var children = new List<Transform>();
                foreach (Transform childTransform in tile.transform)
                {
                    children.Add(childTransform);
                }
                foreach (var childTransform in children)
                {
                    DestroyImmediate(childTransform.gameObject);
                }

                var child = brush.transform.GetChild(0);
                var unit = Instantiate(child.gameObject);
                unit.transform.SetParent(tile.transform);
                unit.transform.position = tile.transform.position;
            }
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
            for (var i = 0; i < map.tiles.transform.childCount; i++)
            {
                Transform t = map.tiles.transform.GetChild(i);
                DestroyImmediate(t.gameObject);
                i--;
            }
        }
    }
}