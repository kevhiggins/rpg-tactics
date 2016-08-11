using System;
using DG.Tweening;
using Rpg.Map;
using UnityEngine;
using Rpg.Unit;

namespace Rpg.Unit
{
    public abstract class AbstractUnit : MonoBehaviour, IUnit
    {
        public int movementSpeed = 4;
        public int speed = 20;
        public Vector2 startPosition;

        private Tile tile;
        private TilePosition startTilePosition;

        public int MovementSpeed
        {
            get
            {
                return movementSpeed;
            }
        }

        public int Speed
        {
            get
            {
                return speed;
            }
        }

        public int ChargeTime
        {
            get;
            private set;
        }

        public TilePosition StartPosition
        {
            get
            {
                if(startTilePosition == null)
                {
                    startTilePosition = new TilePosition((int)startPosition.x, (int)startPosition.y);
                }
                return startTilePosition;
            }
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

            tile.AddUnit(this);
            GetGameObject().transform.position = tile.GetPosition();
        }

        public void StartTurn()
        {
        }

        public void EndTurn()
        {
            ChargeTime = 0;
            GameManager.instance.WaitForNextAction();
        }

        /// <summary>
        /// Increase the units charge time by their speed attribute.
        /// </summary>
        public void ClockTick()
        {
            ChargeTime += Speed;
        }
    }
}
