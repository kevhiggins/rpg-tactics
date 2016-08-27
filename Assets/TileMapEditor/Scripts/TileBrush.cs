
using System.Collections.Generic;
using UnityEngine;

namespace TileMapEditor
{
    public class TileBrush : MonoBehaviour
    {
        public Vector2 brushSize = Vector2.zero;
        public int tileID = 0;
        public SpriteRenderer renderer2D;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, brushSize);
        }

        public void UpdateBrush(Sprite sprite, GameObject unit)
        {
            if (sprite != null)
            {
                var tileMapObject = transform.parent.gameObject;
                var tileMap = tileMapObject.GetComponent<TileMap>();

                var pixelsToUnits = tileMap.pixelsToUnits;

                brushSize = new Vector2(sprite.textureRect.width/pixelsToUnits,
                    sprite.textureRect.height/pixelsToUnits);
            }
            else
            {
                brushSize = Vector2.zero;
            }

            if (unit != null)
            {
                if (transform.childCount != 0)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }

                var unitInstance = Instantiate(unit);
                unitInstance.transform.SetParent(gameObject.transform);
                unitInstance.transform.position = Vector3.zero;
            }
            else
            {
                var children = new List<Transform>();
                foreach (Transform child in transform)
                {
                    children.Add(child);
                }
                foreach (var child in children)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            renderer2D.sprite = sprite;
        }
    }
}
