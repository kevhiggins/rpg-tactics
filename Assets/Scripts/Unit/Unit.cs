using System;
using DG.Tweening;
using Rpg.Map;
using UnityEngine;

namespace Rpg.Unit
{
    class Unit : IUnit
    {
        private GameObject gameObject;
        private Tile tile;

        public Unit(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void SetTile(Tile targetTile)
        {
            tile = targetTile;
        }

        public Tile GetTile()
        {
            return tile;
        }

        public bool HasTile()
        {
            return tile != null;
        }

        public void MoveToTile(Tile tile)
        {
            if (!HasTile())
            {
                throw new Exception("Can not move a unit that is not already on a tile.");
            }

            // Clear unit from previous tile.
            var previousTile = GetTile();
            previousTile.ClearUnit();

            // Add unit to new tile.
            tile.AddUnit(this);

            GetGameObject().transform.DOMove(tile.GetPosition(), 0.5f);
        }

        public void PlaceToTile(Tile tile)
        {
            if (HasTile())
            {
                throw new Exception("Cannot place a unit that is already on a tile.");
            }
            this.tile = tile;
            tile.AddUnit(this);
            GetGameObject().transform.position = tile.GetPosition();
        }
    }
}
