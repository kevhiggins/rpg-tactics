using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

namespace Rpg.PathFinding
{
    class NodeFinder
    {
        public List<IGraphNode> FindNodesInRange(IGraphNode startNode, uint maxDistance, NNConstraint pathConstraint)
        {
            var nodesInRange = new SortedList<int, IGraphNode>();

            // Set startNode tentative distance to zero.
            startNode.TentativeDistance = 0;
            startNode.IsVisited = false;

            nodesInRange.Add(startNode.Id, startNode);

            IGraphNode currentNode;

            while ((currentNode = GetNextUnvisitedNode(nodesInRange)) != null)
            {
                var unvisitedNeighbors = FindUnvisitedNeighbors(currentNode, nodesInRange, pathConstraint);

//                if (currentNode.Id == 88)
//                {
//                    foreach (var node in unvisitedNeighbors)
//                    {
//                        Debug.Log(node.Id);
//                    }
//                }

                foreach (var unvisitedNeighbor in unvisitedNeighbors)
                {
                    unvisitedNeighbor.IsVisited = false;

                    var distanceFromCurrentNode = currentNode.GetConnectionDistance(unvisitedNeighbor);
                    var distanceFromStartNode = currentNode.TentativeDistance + distanceFromCurrentNode;
                    unvisitedNeighbor.TentativeDistance = distanceFromStartNode;

//                    if (currentNode.Id == 88)
//                    {
//                        Debug.Log(currentNode.TentativeDistance);
//                        Debug.Log(unvisitedNeighbor.TentativeDistance);
//                    }

                    // Bug - if we continue at this point, we never see what the best tentative distance was.

                    // If the nodes tentative distance exceeds the maxDistance, then do not add it to the node list.
                    if (unvisitedNeighbor.TentativeDistance > maxDistance)
                        continue;

                    if (unvisitedNeighbor.Id == 88)
                    {
                        Debug.Log(unvisitedNeighbor.TentativeDistance);
                    }

                    // If the node already exists on the list, then assign the tentative distance as the min of the existing, and the newly found.
                    // Otherwise, add the node to the list.
                    if (nodesInRange.ContainsKey(unvisitedNeighbor.Id))
                    {
                        var existingNode = nodesInRange[unvisitedNeighbor.Id];
                        existingNode.TentativeDistance = Math.Min(existingNode.TentativeDistance,
                            unvisitedNeighbor.TentativeDistance);
                    }
                    else
                    {
                        nodesInRange.Add(unvisitedNeighbor.Id, unvisitedNeighbor);
                    }
                }

                currentNode.IsVisited = true;
            }

            return nodesInRange.Values.ToList();
        }

        /// <summary>
        /// Find the neighbors from the node, and check the current node list to make sure they are not visited. Return the unvisited neighbors.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="graphNodes"></param>
        /// <returns></returns>
        public List<IGraphNode> FindUnvisitedNeighbors(IGraphNode node, SortedList<int, IGraphNode> graphNodes, NNConstraint pathConstraint)
        {
            var unvisitedNeighbors = new List<IGraphNode>();
            var neighbors = node.FindNeighbors(pathConstraint);
            foreach (var neighbor in neighbors)
            {
                if (graphNodes.ContainsKey(neighbor.Id))
                {
                    var existingNeighbor = graphNodes[neighbor.Id];
                    if (existingNeighbor.IsVisited == false)
                    {
                        unvisitedNeighbors.Add(existingNeighbor.Clone());
                    }
                }
                else
                {
                    neighbor.IsVisited = false;
                    unvisitedNeighbors.Add(neighbor);
                }
            }
            return unvisitedNeighbors;
        }

        protected IGraphNode GetNextUnvisitedNode(SortedList<int, IGraphNode> graphNodes)
        {
            var sortedNodes = graphNodes.Where(p => p.Value.IsVisited == false).OrderBy(keyValuePair => keyValuePair.Value.TentativeDistance);
            if (!sortedNodes.Any())
            {
                return null;
            }
            return sortedNodes.First().Value;
        }
    }
}