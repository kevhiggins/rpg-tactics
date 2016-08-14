using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.GameState;
using Pathfinding;
using Rpg.Unit;
using UnityEngine;

namespace Rpg.GameState
{
    class EnemyTurn : IGameState
    {
        private IUnit unit;
        private List<IFriendlyUnit> friendlyUnits;
        private int pathsFound = 0;
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
                    (Path path) =>
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

            // 9 => 8  6      


            // Remove the first position, since it is the unit's current location.
            shortestPath.vectorPath.RemoveAt(0);


            var index = shortestPath.vectorPath.Count - 1;
            if (unit.MovementSpeed < shortestPath.vectorPath.Count)
            {
                index -= shortestPath.vectorPath.Count - unit.MovementSpeed;
            }

            var finalPosition = shortestPath.vectorPath[index];

            var map = GameManager.instance.levelManager.GetMap();
            var tile = map.FindTileAtPosition(finalPosition);

            Debug.Log(tile.tilePosition);
            Debug.Log(tile.HasUnit());

            GameManager.instance.GameState = new BlankState();
            unit.MoveToTile(tile, () => { unit.EndTurn(); });

            //            Debug.Log(finalPosition);

            //            Debug.Log(shortestPath.vectorPath.Count);
            //
            //
            //            var targetNode = AstarPath.active.GetNearest().node;
            //
            //            Debug.Log(targetNode.GraphIndex);

            //                        Debug.Log(path.vectorPath.Count);
            //                        Debug.Log(path.vectorPath[0]);
        }

        public void Disable()
        {
        }
    }
}
