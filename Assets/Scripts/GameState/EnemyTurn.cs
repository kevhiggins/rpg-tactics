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

            // Remove the first position, since it is the unit's current location.
            shortestPath.vectorPath.RemoveAt(0);
            // Remove the last position, because we only want to move the enemy next to the target.
            shortestPath.vectorPath.RemoveAt(shortestPath.vectorPath.Count - 1);

            // If the unit is already where it wants to be, then end turn.
            if (shortestPath.vectorPath.Count == 0)
            {
                unit.EndTurn();
                return;
            }

            var index = shortestPath.vectorPath.Count - 1;

            if (unit.MovementSpeed < shortestPath.vectorPath.Count)
            {
                index -= shortestPath.vectorPath.Count - unit.MovementSpeed;
            }

            var finalPosition = shortestPath.vectorPath[index];

            var map = GameManager.instance.levelManager.GetMap();
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
