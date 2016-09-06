using System;
using System.Linq;
using System.Collections.Generic;
using Rpg.PathFinding;
using UnityEngine;
using Rpg.Map;
using GraphPathfinding;

namespace Rpg
{
    public class PathManager : MonoBehaviour
    {
        public Path FindNearestTargetPath(TilePosition sourcePosition, List<TilePosition> targetPositions)
        {
            List<Path> paths = new List<Path>();

            // TODO add overrides for delegates
            var pathFinder = new AStarPathfinder();
            var map = GameManager.instance.levelManager.GetMap();

            var sourceTile = map.GetTile(sourcePosition);

            foreach(var targetPosition in targetPositions)
            {
                var targetTile = map.GetTile(targetPosition);
                var targetPath = pathFinder.FindPath(sourceTile.GraphNode, targetTile.GraphNode);
                if(targetPath != null)
                {
                    paths.Add(targetPath);
                }
            }

            return FindNearestPath(paths);
        }

        private Path FindNearestPath(List<Path> paths)
        {
            if(!paths.Any())
            {
                return null;
            }

            return paths.Aggregate((shortestPath, currentPath) =>
            {
                return (currentPath.Cost < shortestPath.Cost ? currentPath : shortestPath);
            });
        }

    }
}