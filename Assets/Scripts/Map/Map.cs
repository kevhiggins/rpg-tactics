using System;
using Rpg.Unit;
using Tiled2Unity;
using UnityEngine;
using System.Collections.Generic;
using Pathfinding;
using Rpg.PathFinding;

namespace Rpg.Map
{
    public delegate void CursorMoveHandler(TilePosition tilePosition);

    public delegate void TileAddUnitHandler(Tile tile, IUnit unit);

    public delegate void TileRemoveUnitHandler(Tile tile, IUnit unit);

    public class Map
    {
        private GameObject cursorGameObject;
        private TilePosition cursorTilePosition;

        public GameObject GameObject { get; private set; }

        public event CursorMoveHandler OnCursorMove = tilePosition => { };
        public event TileAddUnitHandler OnTileAddUnit = (tile, unit) => { };
        public event TileRemoveUnitHandler OnTileRemoveUnit = (tile, unit) => { };

        public ITileMap TileMap { get; private set; }

        public Map(ITileMap tileMap, GameObject cursorGameObject)
        {
            TileMap = tileMap;
            tileMap.SetMap(this);

            //this.mapGameObject = mapGameObject;
            GameObject = TileMap.GameObject;
            this.cursorGameObject = cursorGameObject;
        }

        public void InitCursor()
        {
            // Set cursor position
            var cursorTile = GetTile(1, 1);
            cursorGameObject.transform.position = cursorTile.GetPosition();
            cursorTilePosition = cursorTile.tilePosition;
        }

        public void SetTileCursor(TilePosition tilePosition)
        {
            MoveTileCursor(tilePosition.x - cursorTilePosition.x, tilePosition.y - cursorTilePosition.y);
        }

        /// <summary>
        /// Moves the tile cursor x and y number of spaces from its current position.
        /// </summary>
        /// <param name="x">The number of tiles on the x axis to move the cursor</param>
        /// <param name="y">The number of tiles on the y axis to move the cursor</param>
        public void MoveTileCursor(int x, int y)
        {
            var tileWidth = TileMap.TileWidth;
            var tileHeight = TileMap.TileHeight;

            var newPosition = cursorGameObject.transform.position;

            var newXTileIndex = cursorTilePosition.x + x;
            var newYTileIndex = cursorTilePosition.y + y;

            // For the x and y axis, check if the new tile position is valid. If so, then update the tile position index and the cursor's position on the screen.
            if (IsValidTilePosition(newXTileIndex, 0))
            {
                newPosition.x += x*tileWidth;
            }
            else
            {
                // If invalid x position, then reset to original position.
                newXTileIndex = cursorTilePosition.x;
            }
            if (IsValidTilePosition(0, newYTileIndex))
            {
                newPosition.y -= y*tileHeight;
            }
            else
            {
                newYTileIndex = cursorTilePosition.y;
            }

            cursorTilePosition = new TilePosition(newXTileIndex, newYTileIndex);

            // StateUpdate the tile cursor position with the newly calculated info.
            cursorGameObject.transform.position = newPosition;

            OnCursorMove(cursorTilePosition);
        }

        public void PlaceUnit(IUnit unit, int x, int y)
        {
            var tile = GetTile(x, y);
            if (tile == null)
            {
                throw new Exception("Could not find tile at position x:" + x + ", y:" + y);
            }

            unit.PlaceToTile(tile);
        }

        public void PlaceUnit(IUnit unit, TilePosition tilePosition)
        {
            PlaceUnit(unit, tilePosition.x, tilePosition.y);
        }

        public void MoveUnitToSelectedTile(IUnit unit, Action onComplete)
        {
            var tile = GetSelectedTile();
            if (tile == null)
            {
                throw new Exception("Could not find tile at cursor position");
            }

            if (tile.HasUnit())
            {
                throw new Exception("Cannot move unit to the same tile as another unit");
            }
            unit.MoveToTile(tile, onComplete);
        }

        public TilePosition GetCursorTilePosition()
        {
            return cursorTilePosition;
        }

        public Tile GetSelectedTile()
        {
            return GetTile(cursorTilePosition.x, cursorTilePosition.y);
        }

        public bool IsValidTilePosition(int x, int y)
        {
            if (x < 0 || x >= TileMap.TilesWide || y < 0 || y >= TileMap.TilesHigh)
            {
                return false;
            }
            return true;
        }

        public Tile GetTile(int x, int y)
        {
            if (IsValidTilePosition(x, y))
            {
                return TileMap.Tiles[x, y];
            }
            return null;
        }

        public Tile GetTile(TilePosition tilePosition)
        {
            return GetTile(tilePosition.x, tilePosition.y);
        }

        /// <summary>
        /// Find the tile at the given world position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Tile FindTileAtPosition(Vector3 position)
        {
            // Subtract a half tile width/height from the position, since it starts on a corner, and tiles are drawn from their centers.
            // This way, if the position was the center of the tile at 0,0, we would shift it's position to the upper left of that tile, for easier maths.
            position -= new Vector3(TileMap.TileWidth/2, -TileMap.TileHeight/2);

            // Find the difference in distance between the map's upper left, and the tiles lower right position.
            var mapPosition = GameObject.transform.position;
            var newPosition = position - mapPosition;


            // Divide the tile dimensions by the point difference to get the number of tiles. Convert to Int32 to avoid float rounding errors.
            int x = Convert.ToInt32(newPosition.x/TileMap.TileWidth);
            int y = Convert.ToInt32(-newPosition.y/TileMap.TileHeight);

            return GetTile(x, y);
        }

        public List<TilePosition> GetTilePositionsInRange(TilePosition targetTilePosition, int range, NNConstraint pathConstraint)
        {
            var targetTile = GetTile(targetTilePosition);

            var targetNode = AstarPath.active.GetNearest(targetTile.GetPosition()).node;
            Debug.Log(targetTile.GetPosition());
            Debug.Log(targetNode.position);
            var nodeFinder = new NodeFinder();
            var nodeAdapter = new GraphNodeAdapter(targetNode);
            var graphNodes = nodeFinder.FindNodesInRange(nodeAdapter, range, pathConstraint);

            var tilesInRange = new List<TilePosition>();

            foreach (var graphNode in graphNodes)
            {
                var tile = FindTileAtPosition((Vector3) graphNode.GraphNode.position);
                tilesInRange.Add(tile.tilePosition);
            }

            return tilesInRange;
        }

        public void TileAddUnit(Tile tile, IUnit unit)
        {
            OnTileAddUnit(tile, unit);
        }

        public void TileRemoveUnit(Tile tile, IUnit unit)
        {
            OnTileRemoveUnit(tile, unit);
        }
    }
}