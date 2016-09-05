using UnityEngine;
using Rpg.Map;
using System.Collections.Generic;
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
    
    // TODO Look into making sure we deallocate resources when map is unloaded
    public void LoadMap()
    {


        var mapInstance = Instantiate(currentMap);
        var mapScript = mapInstance.GetComponent<TileMap>();

        var tileCursorInstance =
            Instantiate(tileSelectionCursor, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

        var tileMap = new TileMapEditorAdapter(mapScript);

        loadedMap = new Map(tileMap, tileCursorInstance);
        

        // Add event handlers to update the walkabliity of nodes on the Astar graph.
//        loadedMap.OnTileAddUnit += UpdateTileWalkability;
//        loadedMap.OnTileRemoveUnit += UpdateTileWalkability;

        // TODO pull this into constructor
        tileMap.ProcessTileData();

        GeneratePathfinding(tileMap);

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
    }

    protected void GeneratePathfinding(ITileMap tileMap)
    {
        foreach (var tile in tileMap.Tiles)
        {
            var graphNode = new GraphNodeTile(tile);
            tile.GraphNode = graphNode;
        }

        foreach (var tile in tileMap.Tiles)
        {
            var neighbors = tile.GetNeighbors();
            foreach (var neighbor in neighbors)
            {
                tile.GraphNode.AddNeighbor(neighbor.GraphNode);
            }
        }
    }
}