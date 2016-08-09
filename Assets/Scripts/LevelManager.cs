using Tiled2Unity;
using UnityEngine;
using Rpg.Map;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject tileSelectionCursor;
    public GameObject highlightedTile;

    private Map loadedMap;

    // TODO Look into making sure we deallocate resources when map is unloaded
    public void LoadMap()
    {
        var mapScript = currentMap.GetComponent<TiledMap>();
        var mapRect = mapScript.GetMapRectInPixelsScaled();

        var mapInstance =
            Instantiate(currentMap, new Vector3(0 - mapRect.width/2, mapRect.height/2, 0), Quaternion.identity) as
                GameObject;
        var tileCursorInstance =
            Instantiate(tileSelectionCursor, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        loadedMap = new Map(mapInstance, tileCursorInstance);
    }

    public Map GetMap()
    {
        return loadedMap;
    }

    public List<GameObject> HighlightTiles(List<TilePosition> tilePositions)
    {
        List<GameObject> highlightedTiles = new List<GameObject>();
        var map = GetMap();
        foreach (var tilePosition in tilePositions)
        {
            var tile = map.GetTile(tilePosition);
            var currentHighlightedTile = Instantiate(highlightedTile, tile.GetPosition(), Quaternion.identity) as GameObject;
            highlightedTiles.Add(currentHighlightedTile);
        }

        return highlightedTiles;
    }
}