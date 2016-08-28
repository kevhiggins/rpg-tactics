﻿using System;
using Tiled2Unity;
using UnityEngine;
using Rpg.Map;
using System.Collections.Generic;
using Pathfinding;
using Rpg.PathFinding;
using Rpg.Unit;
using TileMapEditor;
using Tile = Rpg.Map.Tile;

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


        var mapInstance = Instantiate(currentMap);
        var mapScript = mapInstance.GetComponent<TileMap>();

        var tileCursorInstance =
            Instantiate(tileSelectionCursor, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        var tileMap = new TileMapEditorAdapter(mapScript);

        loadedMap = new Map(tileMap, tileCursorInstance);
        GeneratePathfinding(tileMap);

        // Add event handlers to update the walkabliity of nodes on the Astar graph.
        loadedMap.OnTileAddUnit += UpdateTileWalkability;
        loadedMap.OnTileRemoveUnit += UpdateTileWalkability;

        tileMap.ProcessTileData();
        loadedMap.InitCursor();
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

        AstarPath.RegisterSafeUpdate(() =>
        {
            foreach (var tile in tileMap.Tiles)
            {
                var node = AstarPath.active.GetNearest(tile.GetPosition()).node;
                node.Walkable = tile.IsPassable;
                node.Penalty = (uint)tile.Penalty;
            }
        });


    }
}