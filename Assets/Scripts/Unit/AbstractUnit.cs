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
        public int experience;
        public int experienceWorth;
        public int experienceToLevel = 30;
        public Vector2 startPosition;
        public GameObject deathSound;


        public event LevelUpHandler OnLevelUp = (unit, level) => { };
        public event ExperienceGainHandler OnExperienceGain = (unit, amount) => { };
        public event DamageHandler OnDamage = (unit, amount) => { };
        public event AttackHitHandler OnAttackHit = () => { };
        public event AttackCompleteHandler OnAttackComplete = () => { };
        public event DeathCompleteHandler OnDeathComplete = () => { };

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

        public int Experience
        {
            get { return experience; }
        }

        public int ExperienceWorth
        {
            get { return experienceWorth; }
        }

        public int ExperienceToLevel
        {
            get { return experienceToLevel; }
        }

        public bool IsDead { get; private set; }

        public bool HasMoved { get; private set; }
        public bool HasActed { get; private set; }

        protected AbstractUnit()
        {
            HasMoved = false;
            HasActed = false;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Direction direction = Direction.Right;


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
            HasMoved = true;
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
            else if (startPosition.x > endPosition.x)
                direction = Direction.Left;

            // Change the units facing if needed.
            var scale = GetGameObject().transform.localScale;
            scale.x = Math.Abs(scale.x)*(direction == Direction.Left ? -1 : 1);
            GetGameObject().transform.localScale = scale;
        }

        public void Attack(TilePosition targetPosition)
        {
            CalculateMovementDirection(GetTile().tilePosition, targetPosition);
            TriggerAnimatorParameter("Attack");
            HasActed = true;
        }

        public void Wait()
        {
            HasActed = true;
            HasMoved = true;
        }

        public void Hit()
        {
            TriggerAnimatorParameter("Hit");
        }

        /// <summary>
        /// Called when an attack animation contacts its target.
        /// </summary>
        public void AttackHit()
        {
            OnAttackHit();
        }

        /// <summary>
        /// Called when the attack animation is complete.
        /// </summary>
        public void AttackComplete()
        {
            OnAttackComplete();
        }

        /// <summary>
        /// Called when the death animation is complete.
        /// </summary>
        public void DeathComplete()
        {
            OnDeathComplete();
        }

        public void TakeDamage(int damage)
        {
            Hit();
            currentHp -= damage;
            OnDamage(this, damage);
            if (currentHp < 0)
            {
                currentHp = 0;
            }

            if (currentHp == 0)
            {
                Die();
            }
        }

        public void GainExperience(int amount)
        {
            experience += amount;
            OnExperienceGain(this, amount);
            var hasLeveled = false;
            while (experience >= experienceToLevel)
            {
                level++;
                experience -= experienceToLevel;
                hasLeveled = true;
            }

            // Wait till all levels have happened, so we don't trigger the event more than once.
            if (hasLeveled)
            {
                OnLevelUp(this, level);
            }
        }


        protected void Die()
        {
            TriggerAnimatorParameter("Death");
            GameManager.instance.actionQueue.UnitList.Remove(this);
            GetTile().ClearUnit();
            if (deathSound != null)
            {
                GameManager.instance.audioManager.Play(deathSound);
            }
            IsDead = true;
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
            if (animator == null)
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