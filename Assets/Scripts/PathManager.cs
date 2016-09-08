using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Rpg.Map;
using GraphPathfinding;
using Rpg.PathFinding;
using Rpg.Unit;

namespace Rpg
{
    public class PathManager : MonoBehaviour
    {
        public Path FindNearestTargetPath(TilePosition sourcePosition, List<TilePosition> targetPositions)
        {
            List<Path> paths = new List<Path>();

            var sourceUnit = GameManager.instance.levelManager.GetMap().GetTile(sourcePosition).GetUnit();
            


            var pathFinder = new AStarPathfinder();

            // Customize the movement cost function, and the found node valid function.
            var movementCostFunction = GenerateMovementCostFunction(pathFinder, sourceUnit);

            // Make sure that when a node is found, that if the parent node is occupied, that we will not mark the path as a success.
            FoundNodeValidFunction foundNodeValidFunction = (sourceNode, destinationNode) =>
            {
                var destinationNodeTile = (GraphNodeTile) destinationNode;
                if (destinationNode.Id == sourceNode.Id)
                    return true;
                return !destinationNodeTile.Tile.HasUnit();
            };


            pathFinder.movementCost = movementCostFunction;
            pathFinder.foundNodeValid = foundNodeValidFunction;
            pathFinder.findNodeAdjacentToDestination = true;


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

        public static MovementCostFunction GenerateMovementCostFunction(AStarPathfinder pathFinder, IUnit activeUnit)
        {
            return (sourceNode, destinationNode) =>
            {
                var destinationTileNode = (GraphNodeTile)destinationNode;
                if (!destinationTileNode.Tile.IsPassable)
                    return -1;

                var manhattanCost = AStarPathfinder.ManhattanDistance(sourceNode, destinationNode);

                var cost = manhattanCost + destinationTileNode.Tile.Penalty;

                if (destinationTileNode.Tile.HasUnit())
                {
                    // If the destination has an enemy unit, then it is not passable.
                    if (destinationTileNode.Tile.GetUnit().TeamId != activeUnit.TeamId)
                    {
                        return -1;
                    }

                    // If it is a friendly unit, it is not passable if this is the last tile in a units turn. Add additional cost if it is possible to reach on a future turn.
                    if ((pathFinder.GetNodeCost(sourceNode) + cost) % activeUnit.MovementSpeed == 0)
                    {
                        // Moving the full movement amount this turn will take us exactly to this spot. Need to find an empty space on the path, and increase this nodes cost.
                        var previousParent = (GraphNodeTile)destinationNode;
                        var parentNode = (GraphNodeTile)pathFinder.GetNodeParent(sourceNode);
                        var foundEmptyTile = false;
                        var backtrackCost = 0;

                        while (parentNode != null)
                        {
                            // TODO alternatively, this could be the difference between the node's tentative costs. It's just weird for the latest tile that does not have one yet.
                            backtrackCost += AStarPathfinder.ManhattanDistance(parentNode, previousParent) + previousParent.Tile.Penalty;
                            if (!parentNode.Tile.HasObstacle())
                            {
                                foundEmptyTile = true;
                                break;
                            }

                            previousParent = parentNode;
                            parentNode = (GraphNodeTile)parentNode.ParentNode;
                        }

                        if (foundEmptyTile && backtrackCost != activeUnit.MovementSpeed)
                        {
                            cost += backtrackCost;
                        }
                        else
                        {
                            // The destination is unreachable using this path.
                            return -1;
                        }
                    }
                }

                return cost;
            };
        }
    }
}