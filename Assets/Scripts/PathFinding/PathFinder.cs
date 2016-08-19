using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Rpg.PathFinding
{
    class PathFinder
    {
        private List<Path> paths = new List<Path>();
        private int pathsFound;

        public void FindTargetPaths(Vector3 sourcePosition, List<Vector3> targetPositions, Action<List<Path>> onComplete)
        {
            foreach (var targetPosition in targetPositions)
            {
                var p = ABPath.Construct(sourcePosition,
                    targetPosition,
                    path =>
                    {
                        paths.Add(path);
                        pathsFound++;

                        // Once paths for all units have been found, send them to the onComplete callback.
                        if (pathsFound == targetPositions.Count)
                        {
                            onComplete(paths);
                        }
                    });
                AstarPath.StartPath(p);
            }
        }
    }
}
