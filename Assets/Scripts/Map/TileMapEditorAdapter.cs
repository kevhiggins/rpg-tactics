using System;
using System.Collections.Generic;
using Rpg.Unit;
using TileMapEditor;
using UnityEngine;

namespace Rpg.Map
{
    public class TileMapEditorAdapter : ITileMap
    {
        private TileMap tileMap;

        public TileMapEditorAdapter(TileMap tileMap)
        {
            this.tileMap = tileMap;
        }

        public GameObject GameObject
        {
            get { return tileMap.gameObject; }
        }

        public float TileWidth
        {
            get { return tileMap.tileSize.x/tileMap.pixelsToUnits; }
        }

        public float TileHeight
        {
            get { return tileMap.tileSize.y/tileMap.pixelsToUnits; }
        }

        public int TilesWide
        {
            get { return (int) tileMap.mapSize.x; }
        }

        public int TilesHigh
        {
            get { return (int) tileMap.mapSize.y; }
        }

        public float MapWidth
        {
            get { return tileMap.gridSize.x; }
        }

        public float MapHeight
        {
            get { return tileMap.gridSize.y; }
        }

        public Tile[,] Tiles { get; private set; }
        public List<IUnit> Units { get; private set; }

        private Map map;
        private bool isTileDataProcessed = false;


        public void SetMap(Map map)
        {
            this.map = map;
        }

        public void ProcessTileData()
        {
            if (isTileDataProcessed)
                return;

            // Process tiles/units
            Tiles = new Tile[TilesWide, TilesHigh];
            Units = new List<IUnit>();

            var tilesWide = TilesWide;
            var tilesHigh = TilesHigh;

            for (int x = 0; x < tilesWide; x++)
            {
                for (int y = 0; y < tilesHigh; y++)
                {
                    var tile = new Tile(map, x, y);
                    tile.IsPassable = true;
                    tile.Penalty = 0;
                    Tiles[x, y] = tile;
                }
            }

            var tiles = GameObject.transform.Find("Tiles");
            if (tiles == null)
            {
                throw new Exception("No child tiles found.");
            }

            foreach (Transform tileTransform in tiles.transform)
            {
                var tileScript = tileTransform.gameObject.GetComponent<TileMapEditor.Tile>();
                var tile = Tiles[tileScript.x, tileScript.y];

                tile.IsPassable = tileScript.passable;
                tile.Penalty = tileScript.penalty;

                if (tileTransform.gameObject.transform.childCount > 0)
                {
                    var unitObject = tileTransform.gameObject.transform.GetChild(0).gameObject;
                    var unitScript = unitObject.GetComponent<TileMapEditor.Unit>();

                    var unitInstance = GameObject.Instantiate(unitScript.unitPrefab, tileTransform.position, Quaternion.identity) as GameObject;

                    // Remove the game object attached to the tile.
                    GameObject.Destroy(unitObject);


                    var unit = unitInstance.GetComponent<IUnit>();
                    if (unit == null)
                        throw new Exception("Could not find unit script on unit object.");
                    tile.AddUnit(unit);
                    Units.Add(unit);
                }
            }

            var editorMap = GameObject.GetComponent<TileMap>();
            if (!editorMap.testMode)
            {
                GameObject.DestroyImmediate(tiles.gameObject);
            }
        }
    }
}