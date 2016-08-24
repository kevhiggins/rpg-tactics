using Tiled2Unity;
using UnityEngine;
using Rpg.Map;
using System.Collections.Generic;
using Rpg.PathFinding;
using Rpg.Unit;

public class LevelManager : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject tileSelectionCursor;
    public GameObject highlightedTile;
    public GameObject attackHighlightedTile;

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

        var tileMap = new TiledMapAdapter(mapInstance);

        loadedMap = new Map(tileMap, tileCursorInstance);

        // Add event handlers to update the walkabliity of nodes on the Astar graph.
        loadedMap.OnTileAddUnit += UpdateTileWalkability;
        loadedMap.OnTileRemoveUnit += UpdateTileWalkability;
    }

    public Map GetMap()
    {
        return loadedMap;
    }

    public List<GameObject> HighlightTiles(List<TilePosition> tilePositions, GameObject highlightGameObject)
    {
        List<GameObject> highlightedTiles = new List<GameObject>();
        var map = GetMap();
        foreach (var tilePosition in tilePositions)
        {
            var tile = map.GetTile(tilePosition);
            var currentHighlightedTile =
                Instantiate(highlightGameObject, tile.GetPosition(), Quaternion.identity) as GameObject;
            highlightedTiles.Add(currentHighlightedTile);
        }

        return highlightedTiles;
    }

    protected void UpdateTileWalkability(Tile tile, IUnit unit)
    {
        // Find the GraphNode
        var node = AstarPath.active.GetNearest(tile.GetPosition()).node;

        AstarPath.RegisterSafeUpdate(() =>
        {
            if (tile.HasUnit())
            {
                node.Tag = PathConstraint.TagHasUnit;
            }
            else
            {
                node.Tag = PathConstraint.TagNone;
            }
            
        });
    }
}