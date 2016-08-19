using System.Collections.Generic;
using Rpg.Map;
using Rpg.Unit;
using UnityEngine;

namespace Rpg.Widgets
{
    class HighlightMoveRangeWidget : AbstractWidget
    {
        public List<TilePosition> HighlightedTilePositions { get; private set; }
        private List<GameObject> highlightedTiles;

        public HighlightMoveRangeWidget(IUnit unit)
        {
            var map = GameManager.instance.levelManager.GetMap();

            // Find the tiles in the current units range, and highlight them to show which are available.
            var tilesInRange = map.GetTilePositionsInRange(unit.GetTile().tilePosition, unit.MovementSpeed);
            HighlightedTilePositions = FilterInvalidTilePositions(tilesInRange);

            var levelManager = GameManager.instance.levelManager;
            highlightedTiles = levelManager.HighlightTiles(HighlightedTilePositions, levelManager.highlightedTile);
        }

        public override void Dispose()
        {
            foreach (var highlightedTile in highlightedTiles)
            {
                Object.Destroy(highlightedTile);
            }
        }

        public override void HandleInput()
        {
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
    }
}