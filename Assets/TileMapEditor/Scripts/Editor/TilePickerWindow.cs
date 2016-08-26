using UnityEditor;
using UnityEngine;

namespace TileMapEditor.Editor
{
    class TilePickerWindow : EditorWindow
    {
        public enum Scale
        {
            X1,
            X2,
            X3,
            X4,
            X5
        }

        public enum Mode
        {
            Impassable,
            Penalties,
            Units
        }

        private Scale scale;
        private Vector2 currentSelection = Vector2.zero;
        

        private bool modeGroupEnabled = true;
        private Mode currentMode;

        public Vector2 scrollPosition = Vector2.zero;

        [MenuItem("Window/Tile Picker")]
        public static void OpenTilePickerWindow()
        {
            var window = EditorWindow.GetWindow(typeof(TilePickerWindow));
            var title = new GUIContent();
            title.text = "Tile Picker";
            window.titleContent = title;
        }

        void OnGUI()
        {
            var previousMode = currentMode;

            GUILayout.BeginHorizontal();
            currentMode = GUILayout.Toggle(currentMode == Mode.Impassable, new GUIContent("Impassable"), "Button") ? Mode.Impassable : currentMode;
            currentMode = GUILayout.Toggle(currentMode == Mode.Penalties, new GUIContent("Penalty"), "Button") ? Mode.Penalties : currentMode;
            currentMode = GUILayout.Toggle(currentMode == Mode.Units, new GUIContent("Units"), "Button") ? Mode.Units : currentMode;
            GUILayout.EndHorizontal();

            if (Selection.activeGameObject == null)
                return;
            var selection = Selection.activeGameObject.GetComponent<TileMap>();
            if (selection == null)
                return;

            if (currentMode == Mode.Impassable)
            {
                // On mode change
                if (previousMode != currentMode)
                {
                    // Create the impassable sprite, and assign it as the currently selected sprite.
                    var boxTexture = new Texture2D((int)selection.tileSize.x, (int)selection.tileSize.y);
                    var fillColorArray = boxTexture.GetPixels();

                    for (var i = 0; i < fillColorArray.Length; i++)
                    {
                        fillColorArray[i] = selection.impassableColor;
                    }

                    boxTexture.SetPixels(fillColorArray);
                    boxTexture.Apply();

                    selection.selectedSprite = Sprite.Create(boxTexture, new Rect(0, 0, boxTexture.width, boxTexture.height), new Vector2(0.5f, 0.5f));

                }
                // Allow drawing impassable color.
            }
            else if (currentMode == Mode.Penalties)
            {
                
            }
            else if (currentMode == Mode.Units)
            {
                
            }
        }

        /*
        void OnGUI()
        {
            if (Selection.activeGameObject == null)
                return;

            var selection = Selection.activeGameObject.GetComponent<TileMap>();
            if (selection == null)
                return;

            var texture2D = selection.texture2D;
            if (texture2D == null)
                return;

            scale = (Scale) EditorGUILayout.EnumPopup("Zoom", scale);

            var newScale = ((int) scale) + 1;
            var newTextureSize = new Vector2(texture2D.width, texture2D.height)*newScale;
            var offset = new Vector2(10, 25);

            var scrollBarSize = new Vector2(5, 5);
            var viewPort = new Rect(0, 0, position.width - scrollBarSize.x, position.height - scrollBarSize.y);
            var contentSize = new Rect(0, 0, newTextureSize.x + offset.x, newTextureSize.y + offset.y);

            scrollPosition = GUI.BeginScrollView(viewPort, scrollPosition, contentSize);
            GUI.DrawTexture(new Rect(offset.x, offset.y, newTextureSize.x, newTextureSize.y), texture2D);

            // TODO Remove the + 2 for the borders, since this is due to offset/padding. Need a better way to handle this.
            var tile = (selection.tileSize)*newScale + new Vector2(1, 1)*newScale;

            var grid = new Vector2(newTextureSize.x/tile.x, newTextureSize.y/tile.y);

            var selectionPosition = new Vector2(tile.x*currentSelection.x + offset.x,
                tile.y*currentSelection.y + offset.y);


            var boxTexture = new Texture2D(1, 1);
            boxTexture.SetPixel(0, 0, new Color(0, 0.5f, 1f, 0.4f));
            boxTexture.Apply();

            var style = new GUIStyle(GUI.skin.customStyles[0]);
            style.normal.background = boxTexture;


            GUI.Box(new Rect(selectionPosition.x, selectionPosition.y, tile.x, tile.y), "", style);

            var cEvent = Event.current;
            Vector2 mousePosition = new Vector2(cEvent.mousePosition.x, cEvent.mousePosition.y);
            if (cEvent.type == EventType.mouseDown && cEvent.button == 0)
            {
                currentSelection.x = Mathf.Floor((mousePosition.x - offset.x + scrollPosition.x)/tile.x);
                currentSelection.y = Mathf.Floor((mousePosition.y - offset.y + scrollPosition.y) / tile.y);

                if (currentSelection.x > grid.x - 1)
                    currentSelection.x = grid.x - 1;

                if (currentSelection.y > grid.y - 1)
                    currentSelection.y = grid.y - 1;

                selection.tileID = (int) (currentSelection.x + (currentSelection.y*grid.x) + 1);

                Repaint();
            }

            GUI.EndScrollView();
        }
        */
    }
}