using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphPathfinding
{
    public delegate int MovementCostFunction(IGraphNode sourceNode, IGraphNode destinationNode);

    public delegate int CostFunction(IGraphNode node);

    public delegate int HeuristicFunction(IGraphNode node, IGraphNode goalNode);

    public delegate bool FoundNodeValidFunction(IGraphNode sourceNode, IGraphNode destinationNode);

    // TODO decouple this from Unity A* pathfinder
    public class AStarPathfinder
    {
        protected MovementCostFunction movementCost;
        protected HeuristicFunction heuristic;
        protected FoundNodeValidFunction foundNodeValid;

        public bool includeDestinationNodeInPathCost = true;

        /// <summary>
        /// Create pathfinder class with default functionality.
        /// </summary>
        public AStarPathfinder() : this(null, null, null)
        {
        }

        /// <summary>
        /// Configure the cost functions and heuristic to use a different implementation.
        /// </summary>
        /// <param name="movementCostFunction"></param>
        /// <param name="heuristicFunction"></param>
        /// <param name="foundNodeValid"></param>
        public AStarPathfinder(MovementCostFunction movementCostFunction, HeuristicFunction heuristicFunction, FoundNodeValidFunction foundNodeValid)
        {
            // If specified, use the override functions. Otherwise, use their defaults.
            if (movementCostFunction == null)
                movementCost = ManhattanDistance;
            else
                movementCost = movementCostFunction;

            if (heuristicFunction == null)
                heuristic = ManhattanDistance;
            else
                heuristic = heuristicFunction;

            if (foundNodeValid == null)
                this.foundNodeValid = (sourceNode, destinationNode) => true;
            else
                this.foundNodeValid = foundNodeValid;
        }

        public Path FindPath(IGraphNode startNode, IGraphNode destinationNode)
        {
            return Run(startNode, destinationNode, 0).first;
        }

        public HashSet<IGraphNode> FindNodesInCostRange(IGraphNode startNode, int cost)
        {
            var oldHeuristic = heuristic;
            heuristic = (a, b) => 0;
            var result = Run(startNode, null, cost).second;
            heuristic = oldHeuristic;
            return result;
        }

        protected Tuple<Path, HashSet<IGraphNode>> Run(IGraphNode sourceNode, IGraphNode destinationNode, int maxCost)
        {
            var openNodes = new Dictionary<IGraphNode, int>();
            var closedNodes = new HashSet<IGraphNode>();

            openNodes[sourceNode] = 0;
            sourceNode.TentativeCost = 0;
            var currentNode = sourceNode;

            var pathFound = false;
            while (openNodes.Any())
            {
                currentNode = DequeueNextItem(openNodes);

                // If a max cost is configured, then use it.
                if (maxCost > 0 && currentNode.TentativeCost > maxCost)
                    continue;

                // If we are trying to find a path to a destination, then compare the current node to the destination.
                if (destinationNode != null)
                {
                    // If current node is the destination node, then break out of loop.
                    if (currentNode.Id == destinationNode.Id)
                    {
                        // IF the found node function approves of the node, than we've found the correct node. Otherwise, this node is invalid, so we continue to the next open node.
                        if (foundNodeValid(sourceNode, currentNode))
                        {
                            pathFound = true;
                            break;
                        }
                        else
                            continue;
                    }
                }

                closedNodes.Add(currentNode);

                var neighbors = currentNode.FindNeighbors();
                foreach (var neighbor in neighbors)
                {
                    var movementCostValue = movementCost(currentNode, neighbor);
                    if (movementCostValue == -1)
                    {
                        continue;
                    }

                    var cost = currentNode.TentativeCost + movementCostValue;
                    if (openNodes.ContainsKey(neighbor) && cost < neighbor.TentativeCost)
                        openNodes.Remove(neighbor);
                    if (closedNodes.Contains(neighbor) && cost < neighbor.TentativeCost)
                        closedNodes.Remove(neighbor);
                    if (!openNodes.ContainsKey(neighbor) && !closedNodes.Contains(neighbor))
                    {
                        neighbor.TentativeCost = cost;
                        openNodes.Add(neighbor, cost + heuristic(neighbor, destinationNode));
                        neighbor.ParentNode = currentNode;
                    }
                }
            }

            Path path;

            if (destinationNode != null && pathFound)
            {
                var finalNode = includeDestinationNodeInPathCost ? currentNode : currentNode.ParentNode;
                var tmpNode = currentNode;
                var nodeList = new List<IGraphNode>();

                while (tmpNode != null)
                {
                    nodeList.Add(tmpNode);
                    tmpNode = tmpNode.ParentNode;
                }

                // Reverse list so it goes from start node to goal node.
                nodeList.Reverse();
                path = nodeList.Any() ? new Path(nodeList, finalNode.TentativeCost) : null;
            }
            else
            {
                path = null;
            }

            return new Tuple<Path, HashSet<IGraphNode>>(path, closedNodes);
        }


        protected IGraphNode DequeueNextItem(Dictionary<IGraphNode, int> openList)
        {
            var nextItem = openList.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            openList.Remove(nextItem);
            return nextItem;
        }

        public static int ManhattanDistance(IGraphNode startNode, IGraphNode endNode)
        {
            var dx = Math.Abs(startNode.X - endNode.X);
            var dy = Math.Abs(startNode.Y - endNode.Y);
            var d = 1;
            return d*(dx + dy);
        }
    }
}