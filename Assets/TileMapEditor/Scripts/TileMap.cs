using System.Collections.Generic;
using Assets.TileMapEditor.Scripts;
using Rpg.Map;
using UnityEngine;

namespace TileMapEditor
{
    public class TileMap : MonoBehaviour
    {
        /// <summary>
        /// Number of tiles wide/high
        /// </summary>
        public Vector2 mapSize = new Vector2(20, 10);

        public Texture2D texture2D;
        public Material spriteMaterial;

        /// <summary>
        /// Dimensions of a single tile in pixels
        /// </summary>
        public Vector2 tileSize = new Vector2(32, 32);

        public Object[] spriteReferences;
        public Vector2 gridSize = new Vector2();
        public int pixelsToUnits = 100;

        public Color impassableColor = new Color(232f/255, 75f/255, 23f/255, 1);

        public List<PenaltyColor> penaltyColors = new List<PenaltyColor>();
        public List<GameObject> units = new List<GameObject>();

        public bool testMode = true;

        public Sprite selectedSprite;

        public GameObject tiles;

        void Awake()
        {
            CalculateGridSize();
            var brushTransform = gameObject.transform.FindChild("Brush");
            if(brushTransform != null)
                Destroy(brushTransform);
        }

        public Sprite CurrentTileBrush
        {
            get { return selectedSprite; }
        }

        void OnDrawGizmosSelected()
        {
            var position = transform.position;


            Gizmos.color = Color.grey;
            var row = 0;
            var maxColumns = mapSize.x;
            var total = mapSize.x*mapSize.y;
            var tile = new Vector3(tileSize.x/pixelsToUnits, tileSize.y/pixelsToUnits, 0);
            var offset = new Vector2(tile.x/2, tile.y/2);

            for (var i = 0; i < total; i++)
            {
                var column = i%maxColumns;
                var newX = (column*tile.x) + offset.x + position.x;
                var newY = -(row*tile.y) - offset.y + position.y;
                Gizmos.DrawWireCube(new Vector2(newX, newY), tile);

                if (column == maxColumns - 1)
                {
                    row++;
                }
            }

            Gizmos.color = Color.white;
            var centerX = position.x + gridSize.x/2;
            var centerY = position.y - gridSize.y/2;

            Gizmos.DrawWireCube(new Vector2(centerX, centerY), gridSize);
        }

        public void CalculateGridSize()
        {
            gridSize = new Vector2(tileSize.x/pixelsToUnits*mapSize.x,
                tileSize.y/pixelsToUnits*mapSize.y);
        }
    }
}