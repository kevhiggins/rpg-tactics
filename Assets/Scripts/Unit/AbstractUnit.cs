using System;
using Assets.Scripts.Unity;
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
        public int damage = 10;
        public Vector2 startPosition;

        public enum Direction
        {
            Left,
            Right
        }

        private Tile tile;
        private TilePosition startTilePosition;

        public int MovementSpeed
        {
            get { return movementSpeed; }
        }

        public int Speed
        {
            get { return speed; }
        }

        public int ChargeTime { get; private set; }

        public TilePosition StartPosition
        {
            get
            {
                if (startTilePosition == null)
                {
                    startTilePosition = new TilePosition((int) startPosition.x, (int) startPosition.y);
                }
                return startTilePosition;
            }
        }

        public int CurrentHp
        {
            get { return currentHp; }
        }

        public int MaxHp
        {
            get { return maxHp; }
        }

        public string UnitName
        {
            get { return unitName; }
        }

        public int Level
        {
            get { return level; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private Direction direction;



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

            var previousTile = GetTile();

            CalculateMovementDirection(previousTile.tilePosition, tile.tilePosition);

            // Clear unit from previous tile.
            previousTile.ClearUnit();

            // Add unit to new tile.
            tile.AddUnit(this);

            SetUnitOrderLayerPosition();

            TriggerAnimatorParameter("StartMove");
            GetGameObject().transform.DOMove(tile.GetPosition(), 0.5f).OnComplete(() =>
            {
                TriggerAnimatorParameter("EndMove");
                onComplete();
            });
        }

        /// <summary>
        /// Calculate the new direction of the unit based on the tile movement.
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        public void CalculateMovementDirection(TilePosition startPosition, TilePosition endPosition)
        {
            // If neither condition, then staying the same is what we want.
            if (startPosition.x < endPosition.x)
                direction = Direction.Right;
            else if(startPosition.x > endPosition.x)
                direction = Direction.Left;

            var animator = GetAnimator();
            if(animator != null)
            {
                animator.SetBool("IsFacingLeft", direction == Direction.Left);
            }

            // Change the units facing if needed.
            var scale = GetGameObject().transform.localScale;
            scale.x = Math.Abs(scale.x) * (direction == Direction.Left ? -1 : 1);
            GetGameObject().transform.localScale = scale;
        }

        public void Attack(TilePosition targetPosition)
        {
            CalculateMovementDirection(GetTile().tilePosition, targetPosition);
            TriggerAnimatorParameter("Attack");
        }

        public void Hit()
        {
            TriggerAnimatorParameter("Hit");
        }

        public void TakeDamage(int damage)
        {
            Hit();
            currentHp -= damage;
            if (currentHp < 0)
            {
                currentHp = 0;
            }

            if (currentHp == 0)
            {
                Die();
            }
        }

        protected void Die()
        {
            TriggerAnimatorParameter("Death");
            GameManager.instance.actionQueue.UnitList.Remove(this);
            GetTile().ClearUnit();
        }

        protected void TriggerAnimatorParameter(string parameterName)
        {
            var animator = GetAnimator();
            if (animator != null && animator.HasParameter(parameterName))
            {
                animator.SetTrigger(parameterName);
            }
        }

        protected Animator GetAnimator()
        {
            var animator = GetGameObject().GetComponent<Animator>();
            if(animator == null)
            {
                animator = GetGameObject().GetComponentInChildren<Animator>();
            }
            return animator;
        }

        public SpriteRenderer GetSpriteRenderer()
        {
            // If the GameObject uses animations, then the SpriteRenders is on the GameObject, otherwise, it's on the child object.
            var spriteRenderer = GetGameObject().GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = GetGameObject().GetComponentInChildren<SpriteRenderer>();
            }
            return spriteRenderer;
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
            var spriteRenderer = GetSpriteRenderer();
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
            if ((object) p == null)
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