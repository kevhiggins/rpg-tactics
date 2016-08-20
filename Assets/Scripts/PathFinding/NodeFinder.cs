using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rpg.Unit;

namespace Rpg.PathFinding
{
    class NodeFinder
    {
        public List<IGraphNode> FindNodesInRange(IGraphNode startNode, float maxDistance)
        {
            var graphNodes = new SortedList<int, IGraphNode>();


//            var nodesInRange = new SortedList<TKey,TValue>();

            // Set startNode tentative distance to zero.
            startNode.TentativeDistance = 0;

            var currentNode = startNode;
            graphNodes.Add(currentNode.Id, currentNode);


            var unvisitedNeighbors = currentNode.FindUnvisitedNeighbors();
            foreach (var unvisitedNeighbor in unvisitedNeighbors)
            {
                unvisitedNeighbor.IsVisited = false;

                var distanceFromCurrentNode = currentNode.GetConnectionDistance(unvisitedNeighbor);
                var distanceFromStartNode = currentNode.TentativeDistance + distanceFromCurrentNode;
                unvisitedNeighbor.TentativeDistance = distanceFromStartNode;

                // If the nodes tentative distance exceeds the maxDistance, then do not add it to the node list.
                if (unvisitedNeighbor.TentativeDistance > maxDistance)
                    continue;

                // If the node already exists on the list, then assign the tentative distance as the min of the existing, and the newly found.
                // Otherwise, add the node to the list.
                if (graphNodes.ContainsKey(unvisitedNeighbor.Id))
                {
                    var existingNode = graphNodes[unvisitedNeighbor.Id];
                    existingNode.TentativeDistance = Math.Min(existingNode.TentativeDistance,
                        unvisitedNeighbor.TentativeDistance);
                }
                else
                {
                    graphNodes.Add(unvisitedNeighbor.Id, unvisitedNeighbor);
                }
            }

            currentNode.IsVisited = true;

            // Sort the graph nodes by their tentative distance
            // TODO verify that this works.
            var sortedNodes = graphNodes.Where(p => p.Value.IsVisited == false).OrderBy(keyValuePair => keyValuePair.Value);


            return null;
        }
    }
}