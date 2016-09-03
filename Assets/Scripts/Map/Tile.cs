using System;
using System.Collections.Generic;
using Rpg.PathFinding;
using Rpg.Unit;
using UnityEngine;

namespace Rpg.Map
{
    public class Tile
    {
        public TilePosition tilePosition { get; private set; }
        public bool IsPassable { get; set; }
        public int Penalty { get; set; }
        public GraphNodeTile GraphNode { get; set; }

        public Map Map { get; private set; }
        private IUnit unit;

        public Tile(Map map, int x, int y)
        {
            this.Map = map;
            tilePosition = new TilePosition(x, y);
        }

        public void AddUnit(IUnit targetUnit)
        {
            if (unit != null)
            {
                throw new Exception("Cannot add a targetUnit to a tile that already has a unit in it.");
            }
            unit = targetUnit;
            targetUnit.SetTile(this);
            Map.TileAddUnit(this, targetUnit);
        }

        /// <summary>
        /// Remove the unit from this tile, and clear the tile reference on the unit.
        /// </summary>
        public void ClearUnit()
        {
            if (unit != null)
            {
                var tmpUnit = unit;
                unit.SetTile(null);
                unit = null;
                Map.TileRemoveUnit(this, tmpUnit);
            }
        }

        public bool HasUnit()
        {
            return unit != null;
        }

        public IUnit GetUnit()
        {
            return unit;
        }

        // TODO rename this, it sounds too similar to tilePosition attribute
        public Vector3 GetPosition()
        {
            var currentMapPosition = Map.GameObject.transform.position;
            var tileWidthScaled = Map.TileMap.TileWidth;
            var tileHeightScaled = Map.TileMap.TileHeight;
            var xPosition = currentMapPosition.x + tilePosition.x*tileWidthScaled + tileWidthScaled/2;
            var yPosition = currentMapPosition.y + -tilePosition.y*tileHeightScaled - tileHeightScaled/2;
            return new Vector3(xPosition, yPosition, 0);
        }

        public List<Tile> GetNeighbors()
        {
            var neighbors = new List<Tile>
            {
                Map.GetTile(tilePosition.x - 1, tilePosition.y),
                Map.GetTile(tilePosition.x + 1, tilePosition.y),
                Map.GetTile(tilePosition.x, tilePosition.y - 1),
                Map.GetTile(tilePosition.x, tilePosition.y + 1)
            };
            neighbors.RemoveAll(item => item == null);

            return neighbors;
        }
    }
}