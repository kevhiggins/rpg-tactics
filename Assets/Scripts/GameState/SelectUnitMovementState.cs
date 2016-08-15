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
                if (UnitCanMoveToTilePosition(cursorTilePosition))
                {
                    // TODO Change this to tell the unit it's moving. That way we can decrement the CT gauge appropriately.
                    GameManager.instance.GameState = new BlankState();
                    map.MoveUnitToSelectedTile(unit, () => { unit.EndTurn(); });
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
            var map = GameManager.instance.levelManager.GetMap();

            // Find the tiles in the current units range, and highlight them to show which are available.
            var tilesInRange = map.GetTilePositionsInRange(unit.GetTile().tilePosition, unit.MovementSpeed);
            highlightedTilePositions = FilterInvalidTilePositions(tilesInRange);

            var levelManager = GameManager.instance.levelManager;
            highlightedTiles = levelManager.HighlightTiles(highlightedTilePositions, levelManager.highlightedTile);


            foreach (var highlightedTile in highlightedTiles)
            {
                highlightedTile.SetActive(true);
            }
            base.Enable();
        }

        protected List<TilePosition> FilterInvalidTilePositions(List<TilePosition> tilePositions)
        {
            var map = GameManager.instance.levelManager.GetMap();
            var filteredList = new List<TilePosition>();
            foreach (var tilePosition in tilePositions)
            {
                // Only add the tile to the highlight list if it exists, and does not have a unit.
                var tile = map.GetTile(tilePosition.x, tilePosition.y);
                if (tile != null && tile.HasUnit() == false)
                {
                    filteredList.Add(tilePosition);
                }
            }
            return filteredList;
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
    }
}