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
            var startNode = AstarPath.active.GetNearest(sourcePosition).node;

            foreach (var targetPosition in targetPositions)
            {
                // Find the target node so that we can make sure it's excluded from the constraint, and always traversible in the path.
                var targetNode = AstarPath.active.GetNearest(targetPosition).node;

                var p = (ABPathExclusion) ABPathExclusion.Construct(sourcePosition,
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

                // Gather a list of the source and target nodes.
                var alwaysTraversableNodes = new List<GraphNode>();
                alwaysTraversableNodes.Add(startNode);
                alwaysTraversableNodes.Add(targetNode);

                // Tell constraint to ignore the source and target nodes.
                var pathConstraint = new PathConstraint();
                pathConstraint.alwaysSuitableNodes = alwaysTraversableNodes;

                // Tell the path that the source and target nodes are always traversible.
                p.alwaysTraversableNodes = alwaysTraversableNodes;

                // Constrain movement to basic ground (non-occupied) nodes.
                p.enabledTags = PathConstraint.TagBasicGround;
                p.nnConstraint = pathConstraint;

                AstarPath.StartPath(p);
            }
        }
    }
}
