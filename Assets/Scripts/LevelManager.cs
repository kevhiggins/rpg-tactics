using System;
using Tiled2Unity;
using UnityEngine;
using Rpg.Map;
using System.Collections.Generic;
using Pathfinding;
using Rpg.PathFinding;
using Rpg.Unit;

public class LevelManager : MonoBehaviour
{
    public GameObject currentMap;
    public GameObject tileSelectionCursor;
    public GameObject highlightedTile;
    public GameObject attackHighlightedTile;

    private Map loadedMap;
    private AstarPath astarPathScript;

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
        GeneratePathfinding(tileMap);

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

    protected void GeneratePathfinding(ITileMap tileMap)
    {
        // Create a game object
        var astarObject = new GameObject("Astar Object");
        astarObject.AddComponent<AstarPath>();

        // Set transform
        //        var objectPosition = tileMap.GameObject.transform.position;
        //        var astarPosition = new Vector3(objectPosition.x + tileMap.MapWidth/2, objectPosition.y - tileMap.MapHeight/2, 0);
        //        astarObject.transform.position = astarPosition;

        astarPathScript = astarObject.GetComponent<AstarPath>();

        AstarData data = AstarPath.active.astarData;
        var gridGraph = data.AddGraph(typeof(GridGraph)) as GridGraph;
        if (gridGraph == null)
        {
            throw new Exception("Could not create grid graph.");
        }

        gridGraph.Width = tileMap.TilesWide;
        gridGraph.Depth = tileMap.TilesHigh;
        gridGraph.nodeSize = tileMap.TileWidth;
        gridGraph.neighbours = NumNeighbours.Four;
        gridGraph.initialPenalty = 1;
        gridGraph.rotation = new Vector3(90, 0 , 0);
        gridGraph.collision.collisionCheck = false;
        gridGraph.collision.heightCheck = false;

        gridGraph.UpdateSizeFromWidthDepth();
        AstarPath.active.Scan();


        //        *AstarData data = AstarPath.active.astarData;
        //        *
        //        * // This creates a Grid Graph
        //        *GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
        //        *
        //        * // Setup a grid graph with some values
        //        *gg.width = 50;
        //        *gg.depth = 50;
        //        *gg.nodeSize = 1;
        //        *gg.center = new Vector3(10, 0, 0);
        //        *
        //        * // Updates internal size from the above values
        //        *gg.UpdateSizeFromWidthDepth();
        //        *
        //        * // Scans all graphs, do not call gg.Scan(), that is an internal method
        //        *AstarPath.active.Scan();
    }
}