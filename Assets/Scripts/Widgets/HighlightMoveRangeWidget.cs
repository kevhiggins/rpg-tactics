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
            HighlightedTilePositions = map.GetTilePositionsInRange(unit.GetTile().tilePosition, unit.MovementSpeed);

            // Remove the units current tile position.
            HighlightedTilePositions.Remove(unit.GetTile().tilePosition);

            var deletePositions = new List<TilePosition>();
            foreach (var tilePosition in HighlightedTilePositions)
            {
                if(map.GetTile(tilePosition).HasUnit())
                    deletePositions.Add(tilePosition);
            }

            foreach (var position in deletePositions)
            {
                HighlightedTilePositions.Remove(position);
            }

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
    }
}