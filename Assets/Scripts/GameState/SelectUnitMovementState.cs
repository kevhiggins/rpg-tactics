using System.Collections.Generic;
using Rpg.GameState;
using Rpg.Map;
using Rpg.Unit;
using UnityEngine;

namespace Assets.Scripts.GameState
{
    class SelectUnitMovementState : AbstractMapGameState, IGameState
    {
        private IUnit unit;
        private IGameState parent;
        private List<TilePosition> highlightedTilePositions;
        private List<GameObject> highlightedTiles;

        public SelectUnitMovementState(IGameState parent, IUnit unit)
        {
            this.parent = parent;
            this.unit = unit;
            highlightedTilePositions = GetTilePositionsInRange();
            highlightedTiles = GameManager.instance.levelManager.HighlightTiles(highlightedTilePositions);



            // Highlight the movement tiles
        }

        public new void HandleInput()
        {
            base.HandleInput();

            var inputManager = GameManager.instance.inputManager;
            if (inputManager.Cancel())
            {
                BackToParentState();
            }


            // Go movementspeed Up from selected tile
            // Get single tile
            // Go down a row
            // Get the single tile, and tiles 1 to the right and left
            // Go down a row
            // Get the single tile and tiles 2 to the right/left
        }

        public override void HandleAccept()
        {
            // If the tile is not the current user tile, and the tile is in the list of highlighted tiles, then move the unit.
            var map = GameManager.instance.levelManager.GetMap();
            var cursorTilePosition = map.GetCursorTilePosition();

            if(unit.GetTile().tilePosition != cursorTilePosition)
            {
                if(UnitCanMoveToTilePosition(cursorTilePosition))
                {
                    map.MoveUnitToSelectedTile(unit);
                }
            }
            // TODO override equals for tilePosition

        }

        protected bool UnitCanMoveToTilePosition(TilePosition tilePosition)
        {
            foreach (var highlightedTilePosition in highlightedTilePositions)
            {
                if (tilePosition == highlightedTilePosition)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Enable()
        {
            foreach (var highlightedTile in highlightedTiles)
            {
                highlightedTile.SetActive(true);
            }
        }

        public override void Disable()
        {
            foreach (var highlightedTile in highlightedTiles)
            {
                highlightedTile.SetActive(false);

                // TODO this should be moved into a separate destroy function or something
                Object.Destroy(highlightedTile);
            }

            GameManager.instance.levelManager.GetMap().SetTileCursor(unit.GetTile().tilePosition);
        }

        private void BackToParentState()
        {
            // Disable anything from this state
            // Tell the old state that it's time to return.

            // TODO change the SET method in the game manager to disable old states and enable new states automatically??
            Disable();
            GameManager.instance.GameState = parent;
            parent.Enable();
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
                if (yPosition >= 0 && yPosition < map.TilesHigh())
                {
                    for (int x = -xBreadth; x <= xBreadth; x++)
                    {
                        var xPosition = tilePosition.x + x;
                        if (xPosition < 0 || xPosition >= map.TilesWide())
                        {
                            continue;
                        }
                        movementTilePositions.Add(new TilePosition(xPosition, yPosition));
                    }
                }

                if (y > 0)
                {
                    xBreadth++;
                }
                else
                {
                    xBreadth--;
                }
            }

            return movementTilePositions;
        }
    }

}
