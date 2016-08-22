using System.Collections.Generic;
using Pathfinding;
using Rpg.Map;
using Rpg.PathFinding;
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
            HighlightedTilePositions = map.GetTilePositionsInRange(unit.GetTile().tilePosition, unit.MovementSpeed, PathConstraint.OnlyEmptySpaces);

            // Remove the units current tile position.
            HighlightedTilePositions.Remove(unit.GetTile().tilePosition);

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