using System;
using Rpg.Map;

namespace Rpg.Unit
{
    public interface ITileChild
    {
        void SetTile(Tile tile);
        Tile GetTile();
        bool HasTile();

        void MoveToTile(Tile tile, Action onComplete);
        void PlaceToTile(Tile tile);
    }
}
