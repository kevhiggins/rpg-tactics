using Tiled2Unity;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject tileSelectionCursor;

    [HideInInspector] public GameObject tileSelectionCursorInstance;

    private GameObject currentMapInstance;
    private TiledMap tiledMapScript;
    private TilePosition cursorTilePosition;

    public void LoadMap()
    {
        // Get the TileMap script which contains methods to get the size of the map in unity units.
        tiledMapScript = currentMap.GetComponent<TiledMap>();
        var mapRect = tiledMapScript.GetMapRectInPixelsScaled();

        currentMapInstance = Instantiate(currentMap, new Vector3(0 - mapRect.width / 2, mapRect.height / 2, 0), Quaternion.identity) as GameObject;

        cursorTilePosition = new TilePosition(0, 0);

        tileSelectionCursorInstance =
            Instantiate(tileSelectionCursor, GetTileIndexPosition(cursorTilePosition.x, cursorTilePosition.y), Quaternion.identity) as GameObject;

        tiledMapScript = currentMapInstance.GetComponent<TiledMap>();
    }

    /**
     * Moves the tile cursor x and y number of spaces from its current position.
     */ 
    public void MoveTileCursor(int x, int y)
    {
        var tileWidth = GetTileWidthScaled();
        var tileHeight = GetTileHeightScaled();
        
        //var mapTileWidth = tiledMapScript.NumTilesWide;
        //var mapTileHeight = tiledMapScript.NumTilesHigh;

        var newPosition = tileSelectionCursorInstance.transform.position;

        newPosition.x += x*tileWidth;
        newPosition.y -= y*tileHeight;

        // Use the sides of the map rectangle to bound the position of the cursor.
        // Update the tile cursor position if bounds not exceeded.
        var mapRect = tiledMapScript.GetMapRectInPixelsScaled();   
        if ((newPosition.x > mapRect.xMax) || newPosition.x < mapRect.xMin)
        {
            newPosition.x = tileSelectionCursorInstance.transform.position.x;
        }
        else
        {
            cursorTilePosition.x += x;
        }
        if ((newPosition.y > mapRect.yMax) || newPosition.y < mapRect.yMin)
        {
            newPosition.y = tileSelectionCursorInstance.transform.position.y;
        }
        else
        {
            cursorTilePosition.y += y;
        }

        // Update the tile cursor position with the newly calculated info.
        tileSelectionCursorInstance.transform.position = newPosition;
    }

    public void PlaceHero(GameObject hero, int x, int y)
    {
        hero.transform.position = GetTileIndexPosition(x, y);
    }

    public void MoveHeroToSelectedTile(GameObject hero)
    {
        var tilePosition = GetCursorTilePosition();
        // TODO create a position object or something instead of casting floats to ints
        var newHeroPosition = GetTileIndexPosition(tilePosition.x, tilePosition.y);

        hero.transform.DOMove(newHeroPosition, 0.5f);
    }

    /**
     * Returns the position of the center of the tile being indexed.
     */
    public Vector3 GetTileIndexPosition(int x, int y)
    {
        var currentMapPosition = currentMapInstance.transform.position;
        var xPosition = currentMapPosition.x + x * GetTileWidthScaled() + GetTileWidthScaled() / 2;
        var yPosition = currentMapPosition.y + -y * GetTileHeightScaled() - GetTileHeightScaled() / 2;
        return new Vector3(xPosition, yPosition, 0);
    }

    public float GetTileWidthScaled()
    {
        return tiledMapScript.TileWidth * tiledMapScript.transform.lossyScale.x* tiledMapScript.ExportScale;
    }

    public float GetTileHeightScaled()
    {
        return tiledMapScript.TileHeight * tiledMapScript.transform.lossyScale.y * tiledMapScript.ExportScale;
    }
    
    public TilePosition GetCursorTilePosition()
    {
        return cursorTilePosition;
    }
}