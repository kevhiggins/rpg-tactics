using System;
using System.Collections.Generic;
using Assets.TileMapEditor.Scripts;
using Rpg.Unit;
using UnityEditor;
using UnityEngine;

namespace TileMapEditor.Editor
{
    class TilePickerWindow : EditorWindow
    {
        public enum Mode
        {
            None,
            Impassable,
            Penalties,
            Units
        }

        public delegate void WindowOpenHandler(TilePickerWindow window);

        public static event WindowOpenHandler OnWindowOpen = window => { };


        public delegate void SelectionChangeHandler(Sprite sprite, GameObject unit);

        public event SelectionChangeHandler OnSelectionChange = (sprite, unit) => { };

        public delegate void BeforeDestroyHandler(TilePickerWindow window);

        public event BeforeDestroyHandler BeforeDestroy = window => { };

        public Sprite SelectedSprite { get; private set; }
        public GameObject SelectedUnit { get; private set; }

        public bool forceRefresh = false;

        private Mode currentMode;
        private int selectedPenaltyIndex;
        private int selectedUnitIndex;

        public static TilePickerWindow Instance
        {
            get { return OpenTilePickerWindow(); }
        }

        [MenuItem("Window/Tile Picker")]
        public static TilePickerWindow OpenTilePickerWindow()
        {
            // Open next to game view window if it exists.
            System.Reflection.Assembly assembly = typeof(EditorWindow).Assembly;
            var gameType = assembly.GetType("UnityEditor.GameView");
            var window = GetWindow<TilePickerWindow>("Tile Picker", gameType);

            OnWindowOpen(window);
            return window;
        }

        void OnEnable()
        {
        }

        string[] GetPenaltyOptions()
        {
            var tileMap = ActiveTileMap();
            if (tileMap == null)
                return new string[0];
            var penaltyOptions = new string[tileMap.penaltyColors.Count];
            var index = 0;
            foreach (var penaltyColor in tileMap.penaltyColors)
            {
                penaltyOptions[index] = "Penalty " + penaltyColor.penalty;
                index++;
            }
            return penaltyOptions;
        }

        string[] GetUnitOptions()
        {
            var tileMap = ActiveTileMap();
            if (tileMap == null)
                return new string[0];
            var unitOptions = new string[tileMap.units.Count];
            var index = 0;
            foreach (var unit in tileMap.units)
            {
                unitOptions[index] = unit.name;
                index++;
            }
            return unitOptions;
        }

        void OnGUI()
        {
            // Return if the tilemap inspector is not open.
            var tileMap = ActiveTileMap();
            if (tileMap == null)
                return;

            GUILayout.BeginHorizontal();
            currentMode = GUILayout.Toggle(currentMode == Mode.Impassable, new GUIContent("Impassable"), "Button")
                ? Mode.Impassable
                : currentMode;
            currentMode = GUILayout.Toggle(currentMode == Mode.Penalties, new GUIContent("Penalty"), "Button")
                ? Mode.Penalties
                : currentMode;
            currentMode = GUILayout.Toggle(currentMode == Mode.Units, new GUIContent("Units"), "Button")
                ? Mode.Units
                : currentMode;
            GUILayout.EndHorizontal();

            UpdateSelection();
        }

        public void UpdateSelection()
        {
            var tileMap = ActiveTileMap();
            if (currentMode == Mode.Impassable)
            {
                // On mode change

                // Create the impassable sprite, and assign it as the currently selected sprite.
                SelectedSprite = GetFilledSprite((int) tileMap.tileSize.x, (int) tileMap.tileSize.y,
                    tileMap.impassableColor);
                OnSelectionChange(SelectedSprite, null);
            }
            else if (currentMode == Mode.Penalties)
            {
                selectedPenaltyIndex = EditorGUILayout.Popup("Penalties:", selectedPenaltyIndex, GetPenaltyOptions());

                if (tileMap.penaltyColors.Count > 0)
                {
                    var penaltyColor = tileMap.penaltyColors[selectedPenaltyIndex].color;
                    SelectedSprite = GetFilledSprite((int) tileMap.tileSize.x, (int) tileMap.tileSize.y, penaltyColor);
                }
                else
                {
                    SelectedSprite = null;
                }

                OnSelectionChange(SelectedSprite, null);
            }
            else if (currentMode == Mode.Units)
            {
                selectedUnitIndex = EditorGUILayout.Popup("Units:", selectedUnitIndex, GetUnitOptions());

//                var penaltyColor = tileMap.penaltyColors[selectedPenaltyIndex].color;
//                SelectedSprite = GetFilledSprite((int)tileMap.tileSize.x, (int)tileMap.tileSize.y, penaltyColor);

                // Get unit

                if (tileMap.units.Count > 0)
                    SelectedUnit = tileMap.units[selectedUnitIndex];
                else
                {
                    SelectedUnit = null;
                }

                OnSelectionChange(null, SelectedUnit);
            }
        }

        private PenaltyColor GetPenaltyColor()
        {
            return ActiveTileMap().penaltyColors[selectedPenaltyIndex];
        }

        public Sprite GetFilledSprite(int width, int height, Color color)
        {
            // Create the impassable sprite, and assign it as the currently selected sprite.
            var boxTexture = new Texture2D(width, height);
            var fillColorArray = boxTexture.GetPixels();

            for (var i = 0; i < fillColorArray.Length; i++)
            {
                fillColorArray[i] = color;
            }

            boxTexture.SetPixels(fillColorArray);
            boxTexture.Apply();

            return Sprite.Create(boxTexture,
                new Rect(0, 0, boxTexture.width, boxTexture.height), new Vector2(0.5f, 0.5f));
        }

        private TileMap ActiveTileMap()
        {
            if (Selection.activeGameObject == null)
                return null;
            var selection = Selection.activeGameObject.GetComponent<TileMap>();
            return selection;
        }

        void OnDestroy()
        {
            BeforeDestroy(this);
        }

        public void UpdateTile(Tile tile)
        {
            if (currentMode == Mode.Impassable)
            {
                tile.passable = false;
                tile.color = ActiveTileMap().impassableColor;
            }
            else if (currentMode == Mode.Penalties)
            {
                var penaltyColor = GetPenaltyColor();
                tile.passable = true;
                tile.penalty = penaltyColor.penalty;
                tile.color = penaltyColor.color;
            }
        }
    }
}