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

        /// <summary>
        /// Dimensions of a single tile in pixels
        /// </summary>
        public Vector2 tileSize;

        public Object[] spriteReferences;
        public Vector2 gridSize = new Vector2();
        public float pixelsToUnits;
        public int tileID = 0;

        public Sprite CurrentTileBrush
        {
            get { return spriteReferences[tileID] as Sprite; }
        }

        void OnDrawGizmosSelected()
        {
            var position = transform.position;

            if (texture2D != null)
            {
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
        }
    }
}