using System;
using System.Linq;
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
            int shortestDistance = 0;

            foreach (var path in paths)
            {
                var distance = PathDistance(path.vectorPath);

                if (shortestPath == null || shortestDistance > distance)
                {
                    shortestPath = path;
                    shortestDistance = distance;
                }
            }

            if (shortestPath == null)
            {
                throw new Exception("Could not find shortest path.");
            }

            onComplete(shortestPath);
        }

        private int PathDistance(List<Vector3> vectorPaths)
        {
            int distance = 0;
            var nodePositions = vectorPaths.Skip(1);
            foreach (var nodePosition in nodePositions)
            {
                var node = AstarPath.active.GetNearest(nodePosition).node;
                distance += (int) node.Penalty;
            }
            return distance;
        }
    }
}