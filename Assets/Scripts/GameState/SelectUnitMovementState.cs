﻿using System;
using System.Collections.Generic;
using Rpg.GameState;
using Rpg.Map;
using Rpg.Unit;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.GameState
{
    class SelectUnitMovementState : AbstractMapGameState, IGameState, IDisposable
    {
        private IUnit unit;
        private IGameState parent;
        private List<TilePosition> highlightedTilePositions;
        private List<GameObject> highlightedTiles;

        public SelectUnitMovementState(IGameState parent, IUnit unit)
        {
            this.parent = parent;
            this.unit = unit;

            // Find the tiles in the current units range, and highlight them to show which are available.
            highlightedTilePositions = GetTilePositionsInRange();
            highlightedTiles = GameManager.instance.levelManager.HighlightTiles(highlightedTilePositions);
        }

        public void Dispose()
        {
            foreach (var highlightedTileObject in highlightedTiles)
            {
                Object.Destroy(highlightedTileObject);
            }
        }


        public new void HandleInput()
        {
            base.HandleInput();

            var inputManager = GameManager.instance.inputManager;

            // On Cancel command, go back to the parent menu.
            if (inputManager.Cancel())
            {
                BackToParentState();
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
                    map.MoveUnitToSelectedTile(unit);
                    BackToParentState();
                }
            }
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
//                Object.Destroy(highlightedTile);
            }

            GameManager.instance.levelManager.GetMap().SetTileCursor(unit.GetTile().tilePosition);
        }

        private void BackToParentState()
        {
            // Switch back to the parent state.
            GameManager.instance.GameState = parent;
            Dispose();
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
