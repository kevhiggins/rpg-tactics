using System;
using System.Collections.Generic;
using Assets.Scripts.GameState;
using Pathfinding;
using Rpg.Unit;

namespace Rpg.GameState
{
    class EnemyTurn : IGameState
    {
        private IUnit unit;
        private List<IFriendlyUnit> friendlyUnits;
        private int pathsFound;
        private List<Path> paths = new List<Path>();

        public EnemyTurn(IUnit unit)
        {
            this.unit = unit;
            friendlyUnits = GameManager.instance.actionQueue.GetFriendlyUnits();
        }

        public void HandleInput()
        {
        }

        public void Enable()
        {
            foreach (var friendlyUnit in friendlyUnits)
            {
                var p = ABPath.Construct(unit.GetGameObject().transform.position,
                    friendlyUnit.GetGameObject().transform.position,
                    path =>
                    {
                        paths.Add(path);
                        pathsFound++;

                        // Once paths for all units have been found, then process the paths.
                        if (pathsFound == friendlyUnits.Count)
                        {
                            ProcessPaths();
                        }

                    });
                AstarPath.StartPath(p);
            }
        }

        protected void ProcessPaths()
        {
            Path shortestPath = null;

            foreach (var path in paths)
            {
                if (shortestPath == null)
                {
                    shortestPath = path;
                }
                else if (shortestPath.vectorPath.Count > path.vectorPath.Count)
                {
                    shortestPath = path;
                }
            }

            if (shortestPath == null)
            {
                throw new Exception("Could not find shortest path.");
            }

            var map = GameManager.instance.levelManager.GetMap();

            // Remove the first position, since it is the unit's current location.
            shortestPath.vectorPath.RemoveAt(0);
            // Remove the last position, because we only want to move the enemy next to the target.
            var targetIndex = shortestPath.vectorPath.Count - 1;
            var targetPosition = shortestPath.vectorPath[targetIndex];
            shortestPath.vectorPath.RemoveAt(targetIndex);

            // If the unit is already where it wants to be, then end turn.
            if (shortestPath.vectorPath.Count == 0)
            {
                // Get the target tile's unit and attack it.
                var unitTile = map.FindTileAtPosition(targetPosition);
                if (unitTile.HasUnit() == false)
                {
                    throw new Exception("Could not find a unit at the tile we are targeting. This isn't supposed to be possible.");
                }
                var targetUnit = unitTile.GetUnit();


                GameManager.instance.battleManager.AttackUnit(unit, targetUnit);
                return;
            }

            var index = shortestPath.vectorPath.Count - 1;

            if (unit.MovementSpeed < shortestPath.vectorPath.Count)
            {
                index -= shortestPath.vectorPath.Count - unit.MovementSpeed;
            }

            var finalPosition = shortestPath.vectorPath[index];

            
            var tile = map.FindTileAtPosition(finalPosition);

            // Wait for the unit to finish moving, and then end the turn.
            GameManager.instance.GameState = new BlankState();
            unit.MoveToTile(tile, () => { unit.EndTurn(); });
        }

        public void Disable()
        {
        }
    }
}
