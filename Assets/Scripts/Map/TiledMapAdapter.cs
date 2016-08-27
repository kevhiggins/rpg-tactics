using System;
using System.Collections.Generic;
using Rpg.Unit;
using Tiled2Unity;
using UnityEngine;

namespace Rpg.Map
{
    class TiledMapAdapter : ITileMap
    {
        public GameObject GameObject { get; private set; }

        public float TileWidth
        {
            get { return tiledMapScript.TileWidth*tiledMapScript.transform.lossyScale.x*tiledMapScript.ExportScale; }
        }

        public float TileHeight
        {
            get { return tiledMapScript.TileHeight*tiledMapScript.transform.lossyScale.y*tiledMapScript.ExportScale; }
        }

        public int TilesWide
        {
            get { return tiledMapScript.NumTilesWide; }
        }

        public int TilesHigh
        {
            get { return tiledMapScript.NumTilesHigh; }
        }

        public float MapWidth
        {
            get { return (float) tiledMapScript.MapWidthInPixels/GameManager.instance.pixelsToUnits; }
        }

        public float MapHeight
        {
            get { return (float) tiledMapScript.MapHeightInPixels/GameManager.instance.pixelsToUnits; }
        }

        public Tile[,] Tiles { get; private set; }
        public List<IUnit> Units { get; private set; }


        public List<Tuple<IUnit, TilePosition>> UnitPositions { get; private set; }

        private TiledMap tiledMapScript;
        private Map map;

        public TiledMapAdapter(GameObject tiledGameObject)
        {
            GameObject = tiledGameObject;
            tiledMapScript = GameObject.GetComponent<TiledMap>();

            // Populate UnitPositions
            // Create a list of the tiles with their penalties.
            // Create a list of the impassable tiles.
        }

        public void SetMap(Map map)
        {
            this.map = map;
        }

        public void ProcessTileData()
        {
            throw new NotImplementedException();
        }
    }
}