using System.Collections.Generic;
using Rpg.GameState;
using Rpg.Map;
using Rpg.Unit;
using UnityEngine;

namespace Assets.Scripts.GameState
{
    class SelectUnitMovementState : IGameState
    {
        private IUnit unit;

        public SelectUnitMovementState(IUnit unit)
        {
            this.unit = unit;
            GetTilePositionsInRange();
            // Highlight the movement tiles
        }

        public void HandleInput()
        {



            // Go movementspeed Up from selected tile
            // Get single tile
            // Go down a row
            // Get the single tile, and tiles 1 to the right and left
            // Go down a row
            // Get the single tile and tiles 2 to the right/left
        }

        public List<TilePosition> GetTilePositionsInRange()
        {
            var tilePosition = unit.GetTile().tilePosition;
            var movementTilePositions = new List<TilePosition>();

            var map = GameManager.instance.levelManager.GetMap();

            var xBreadth = 0;
            for (int y = unit.MovementSpeed; y >= -unit.MovementSpeed; y--)
            {
                var yPosition = tilePosition.y - y;
                if (yPosition < 0 || yPosition >= map.TilesHigh())
                {
                    continue;
                }
                for (int x = -xBreadth; x <= xBreadth; x++)
                {
                    var xPosition = tilePosition.x + x;
                    if (xPosition < 0 || xPosition >= map.TilesWide())
                    {
                        continue;
                    }
                    movementTilePositions.Add(new TilePosition(xPosition, yPosition));
                }
                if (yPosition > 0)
                {
                    xBreadth++;
                }
                else
                {
                    xBreadth--;
                }
            }
            Debug.Log(movementTilePositions.Count);
            return movementTilePositions;
        }
    }
}
