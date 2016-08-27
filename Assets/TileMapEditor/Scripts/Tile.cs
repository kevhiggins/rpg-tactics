using UnityEngine;

namespace TileMapEditor
{
    public class Tile : MonoBehaviour
    {
        public int penalty = 1;
        public bool passable = true;
        public int x;
        public int y;
        public Color color;

        void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer.sprite == null && color != null)
            {
                var tileMap = gameObject.transform.parent.parent.gameObject.GetComponent<TileMap>();
                spriteRenderer.sprite = GetFilledSprite((int)tileMap.tileSize.x, (int)tileMap.tileSize.y, color);
            }
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
    }
}
