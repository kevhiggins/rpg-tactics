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

        private TiledMap tiledMapScript;

        public TiledMapAdapter(GameObject tiledGameObject)
        {
            GameObject = tiledGameObject;
            tiledMapScript = GameObject.GetComponent<TiledMap>();
        }
    }
}