﻿using System;
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

        public void SetTileCursor(TilePosition tilePosition)
        {
            MoveTileCursor(tilePosition.x - cursorTilePosition.x, tilePosition.y - cursorTilePosition.y);
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
                newPosition.x += x*tileWidth;
            }
            else
            {
                // If invalid x position, then reset to original position.
                newXTileIndex = cursorTilePosition.x;
            }
            if (IsValidTilePosition(0, newYTileIndex))
            {
                newPosition.y -= y*tileHeight;
            }
            else
            {
                newYTileIndex = cursorTilePosition.y;
            }

            cursorTilePosition = new TilePosition(newXTileIndex, newYTileIndex);

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

        public void PlaceUnit(IUnit unit, TilePosition tilePosition)
        {
            PlaceUnit(unit, tilePosition.x, tilePosition.y);
        }

        public void MoveUnitToSelectedTile(IUnit unit, Action onComplete)
        {
            var tile = GetSelectedTile();
            if (tile == null)
            {
                throw new Exception("Could not find tile at cursor position");
            }
            
            if(tile.HasUnit())
            {
                throw new Exception("Cannot move unit to the same tile as another unit");
            }
            unit.MoveToTile(tile, onComplete);
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

        public Tile GetTile(TilePosition tilePosition)
        {
            return GetTile(tilePosition.x, tilePosition.y);
        }

        // Find the unity at the unity world position.
        public Tile FindTileAtPosition(Vector3 position)
        {
            // Add a half tile width/height to the position, since it starts on a corner, and tiles are drawn from their centers.
            position += new Vector3(GetTileWidthScaled() / 2, -GetTileHeightScaled() / 2);

            // Find the difference in distance between the map's upper left, and the tiles lower right position.
            var mapPosition = GameObject.transform.position;
            var newPosition = position - mapPosition;

            // Divide the tile dimensions by the point difference to get the number of tiles
            int x = (int)(newPosition.x/GetTileWidthScaled());
            int y = (int)(-newPosition.y/GetTileHeightScaled());

            Debug.Log(x);
            Debug.Log(y);

            return GetTile(x, y);
        }
    }
}