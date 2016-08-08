using System;
using Rpg.Unit;
using Tiled2Unity;
using UnityEngine;

namespace Rpg.Map
{
    public class Map
    {
        private GameObject cursorGameObject;
        private TiledMap tiledMapScript;
        private TilePosition cursorTilePosition;
        private Tile[,] tiles;


        public GameObject GameObject { get; private set; }

        public Map(GameObject mapGameObject, GameObject cursorGameObject)
        {
            //this.mapGameObject = mapGameObject;
            GameObject = mapGameObject;
            this.cursorGameObject = cursorGameObject;

            // Store a reference to the map script
            tiledMapScript = GameObject.GetComponent<TiledMap>();

            // Create Tile object instances for each tile on the map.
            tiles = new Tile[tiledMapScript.NumTilesWide, tiledMapScript.NumTilesHigh];

            var tilesWide = TilesWide();
            var tilesHigh = TilesHigh();

            for (int x = 0; x < tilesWide; x++)
            {
                for (int y = 0; y < tilesHigh; y++)
                {
                    tiles[x, y] = new Tile(this, x, y);
                }
            }

            // Set cursor position
            var cursorTile = GetTile(1, 1);
            cursorGameObject.transform.position = cursorTile.GetPosition();
            cursorTilePosition = cursorTile.tilePosition;
        }


        /// <summary>
        /// Moves the tile cursor x and y number of spaces from its current position.
        /// </summary>
        /// <param name="x">The number of tiles on the x axis to move the cursor</param>
        /// <param name="y">The number of tiles on the y axis to move the cursor</param>
        public void MoveTileCursor(int x, int y)
        {
            var tileWidth = GetTileWidthScaled();
            var tileHeight = GetTileHeightScaled();

            var newPosition = cursorGameObject.transform.position;

            var newXTileIndex = cursorTilePosition.x + x;
            var newYTileIndex = cursorTilePosition.y + y;

            // For the x and y axis, check if the new tile position is valid. If so, then update the tile position index and the cursor's position on the screen.
            if (IsValidTilePosition(newXTileIndex, 0))
            {
                cursorTilePosition.x = newXTileIndex;
                newPosition.x += x*tileWidth;
            }
            if (IsValidTilePosition(0, newYTileIndex))
            {
                cursorTilePosition.y = newYTileIndex;
                newPosition.y -= y*tileHeight;
            }


            // Update the tile cursor position with the newly calculated info.
            cursorGameObject.transform.position = newPosition;
        }

        public void PlaceUnit(IUnit unit, int x, int y)
        {
            var tile = GetTile(x, y);
            if (tile == null)
            {
                throw new Exception("Could not find tile at position x:" + x + ", y:" + y);               
            }

            unit.PlaceToTile(tile);
        }

        public void MoveUnitToSelectedTile(IUnit unit)
        {
            var tile = GetSelectedTile();
            if (tile == null)
            {
                throw new Exception("Could not find tile at cursor position");
            }
            unit.MoveToTile(tile);
        }

        public float GetTileWidthScaled()
        {
            return tiledMapScript.TileWidth*tiledMapScript.transform.lossyScale.x*tiledMapScript.ExportScale;
        }

        public float GetTileHeightScaled()
        {
            return tiledMapScript.TileHeight*tiledMapScript.transform.lossyScale.y*tiledMapScript.ExportScale;
        }

        public TilePosition GetCursorTilePosition()
        {
            return cursorTilePosition;
        }

        public Tile GetSelectedTile()
        {
            return GetTile(cursorTilePosition.x, cursorTilePosition.y);
        }

        public bool IsValidTilePosition(int x, int y)
        {
            if (x < 0 || x >= TilesWide() || y < 0 || y >= TilesHigh())
            {
                return false;
            }
            return true;
        }

        public int TilesWide()
        {
            return tiles.GetLength(0);
        }

        public int TilesHigh()
        {
            return tiles.GetLength(1);
        }

        public Tile GetTile(int x, int y)
        {
            if (IsValidTilePosition(x, y))
            {
                return tiles[x, y];
            }
            return null;
        }
    }
}