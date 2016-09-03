using GraphPathfinding;
using Rpg.Map;

namespace Rpg.PathFinding
{
    public class GraphNodeTile : AbstractGraphNode
    {
        public override int Id { get { return id; } }

        public sealed override int X
        {
            get { return Tile.tilePosition.x; }
        }

        public sealed override int Y
        {
            get { return Tile.tilePosition.y; }
        }

        public Tile Tile { get; set; }
        private int id;

        public GraphNodeTile(Tile tile)
        {
            Tile = tile;
            id = Y*tile.Map.TileMap.TilesWide + X;
        }
    }
}