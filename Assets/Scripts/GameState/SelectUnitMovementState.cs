using System;
using System.Collections.Generic;
using Rpg.Map;
using Rpg.Unit;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.GameState
{
    class SelectUnitMovementState : ExploreMapState
    {
        private IUnit unit;
        private List<TilePosition> highlightedTilePositions;
        private List<GameObject> highlightedTiles;

        public SelectUnitMovementState(IUnit unit) : base(unit)
        {
            this.unit = unit;
        }

        public void Dispose()
        {
            foreach (var highlightedTileObject in highlightedTiles)
            {
                Object.Destroy(highlightedTileObject);
            }
        }


        public override void HandleAccept()
        {
            var map = GameManager.instance.levelManager.GetMap();
            var cursorTilePosition = map.GetCursorTilePosition();

            // If the tile is not the current user tile, and the tile is in the list of highlighted tiles, then move the unit.
            if (!unit.GetTile().tilePosition.Equals(cursorTilePosition))
            {
                if(UnitCanMoveToTilePosition(cursorTilePosition))
                {
                    // TODO Change this to tell the unit it's moving. That way we can decrement the CT gauge appropriately.
                    GameManager.instance.GameState = new BlankState();
                    map.MoveUnitToSelectedTile(unit, () => { unit.EndTurn();});
                }
            }
        }

        public override void HandleCancel()
        {
            BackToParentState();
        }

        /// <summary>
        /// Returns whether or not the unit can move to the selected tile position.
        /// </summary>
        /// <param name="tilePosition"></param>
        /// <returns></returns>
        protected bool UnitCanMoveToTilePosition(TilePosition tilePosition)
        {
            foreach (var highlightedTilePosition in highlightedTilePositions)
            {
                if (tilePosition.Equals(highlightedTilePosition))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Enable()
        {
            // Find the tiles in the current units range, and highlight them to show which are available.
            highlightedTilePositions = GetTilePositionsInRange();
            highlightedTiles = GameManager.instance.levelManager.HighlightTiles(highlightedTilePositions);

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
            }
            Dispose();
            base.Disable();
        }

        private void BackToParentState()
        {
            GameManager.instance.GameState = new ActiveUnitMenuState(unit);
        }


        /// <summary>
        /// Finds a list of the tile positions in movement range of the current unit.
        /// </summary>
        /// <returns></returns>
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

                        // Only add the tile to the highlight list if it exists, and does not have a unit.
                        var tile = map.GetTile(xPosition, yPosition);
                        if (tile != null && tile.HasUnit() == false)
                        {
                            movementTilePositions.Add(new TilePosition(xPosition, yPosition));
                        }
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
