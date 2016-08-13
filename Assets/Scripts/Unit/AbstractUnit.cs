﻿using System;
using DG.Tweening;
using Rpg.Map;
using UnityEngine;

namespace Rpg.Unit
{
    public abstract class AbstractUnit : MonoBehaviour, IUnit
    {
        public string unitName = "UNNAMED";
        public int currentHp = 20;
        public int maxHp = 20;
        public int level = 1;
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
                if (startTilePosition == null)
                {
                    startTilePosition = new TilePosition((int)startPosition.x, (int)startPosition.y);
                }
                return startTilePosition;
            }
        }

        public int CurrentHp
        {
            get
            {
                return currentHp;
            }
        }

        public int MaxHp
        {
            get
            {
                return maxHp;
            }
        }

        public string UnitName
        {
            get
            {
                return unitName;
            }
        }

        public int Level
        {
            get
            {
                return level;
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

        public void MoveToTile(Tile tile, Action onComplete)
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

            SetUnitOrderLayerPosition();

            GetGameObject().transform.DOMove(tile.GetPosition(), 0.5f).OnComplete(() => { onComplete(); });
        }

        public void PlaceToTile(Tile tile)
        {
            if (HasTile())
            {
                throw new Exception("Cannot place a unit that is already on a tile.");
            }

            tile.AddUnit(this);
            SetUnitOrderLayerPosition();
            GetGameObject().transform.position = tile.GetPosition();
        }

        protected void SetUnitOrderLayerPosition()
        {
            var spriteObject = GetGameObject().transform.GetChild(0).gameObject;
            var spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = tile.tilePosition.y;
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

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            AbstractUnit p = obj as AbstractUnit;
            // Return true if the fields match:
            return Equals(p);
        }

        public bool Equals(AbstractUnit p)
        {
            // If parameter is null return false:
            if ((object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return p.GetGameObject().GetInstanceID() == GetGameObject().GetInstanceID();
        }

        public override int GetHashCode()
        {
            return GetGameObject().GetInstanceID();
        }
    }
}
