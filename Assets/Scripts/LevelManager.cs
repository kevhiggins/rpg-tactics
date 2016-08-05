using Tiled2Unity;
using UnityEngine;
using DG.Tweening;
using Rpg.Map;

public class LevelManager : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject tileSelectionCursor;

    private Map loadedMap;

    // TODO Look into making sure we deallocate resources when map is unloaded
    public void LoadMap()
    {
        var mapScript = currentMap.GetComponent<TiledMap>();
        var mapRect = mapScript.GetMapRectInPixelsScaled();

        var mapInstance = Instantiate(currentMap, new Vector3(0 - mapRect.width / 2, mapRect.height / 2, 0), Quaternion.identity) as GameObject;
        var tileCursorInstance =
            Instantiate(tileSelectionCursor, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        loadedMap = new Map(mapInstance, tileCursorInstance);
    }

    public Map GetMap()
    {
        return loadedMap;
    }
}