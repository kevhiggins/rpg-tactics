using Tiled2Unity;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	public GameObject currentMap;
	public GameObject tileSelectionCursor;

	[HideInInspector]
	public GameObject tileSelectionCursorInstance;
    
    private GameObject currentMapInstance;
    private TiledMap tiledMapScript;

	public void LoadMap()
	{
		currentMapInstance = Instantiate(currentMap, Vector3.zero, Quaternion.identity) as GameObject;
		tileSelectionCursorInstance = Instantiate(tileSelectionCursor, new Vector3(16, -16, 0), Quaternion.identity) as GameObject;

        tiledMapScript = currentMapInstance.GetComponent<TiledMap>();
	}

	public void MoveTileCursor(int x, int y)
	{
		// TODO Define a global variable for tile width
		var tileWidth = tiledMapScript.TileWidth;
		var tileHeight = tiledMapScript.TileHeight;

        var mapTileWidth = tiledMapScript.NumTilesWide;
		var mapTileHeight = tiledMapScript.NumTilesHigh;

		var newPosition = tileSelectionCursorInstance.transform.position;
		newPosition.x += x * tileWidth;
		newPosition.y += y * tileHeight;

		if ((newPosition.x > mapTileWidth * tileWidth) || newPosition.x < 0)
		{
			newPosition.x = tileSelectionCursorInstance.transform.position.x;
		}

		if ((newPosition.y < -(mapTileHeight * tileHeight)) || newPosition.y > 0)
		{
			newPosition.y = tileSelectionCursorInstance.transform.position.y;
		}
		tileSelectionCursorInstance.transform.position = newPosition;
	}
}
