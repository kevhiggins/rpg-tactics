using System;
using System.Collections.Generic;
using Pathfinding;
using Rpg.PathFinding;
using UnityEngine;

namespace Rpg
{
    public class PathManager : MonoBehaviour
    {
        public void FindNearestTargetPath(Vector3 sourcePosition, List<Vector3> targetPositions, Action<Path> onComplete)
        {
            var pathFinder = new PathFinder();
            pathFinder.FindTargetPaths(sourcePosition, targetPositions,
                paths => { FindNearestPath(paths, onComplete); });
        }

        private void FindNearestPath(List<Path> paths, Action<Path> onComplete)
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

            onComplete(shortestPath);
        }
    }
}