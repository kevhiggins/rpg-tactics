using System.Collections.Generic;
using Pathfinding;

namespace Rpg.PathFinding
{
    class GraphNodeAdapter : IGraphNode
    {
        public int Id
        {
            get { return node.NodeIndex; }
        }

        public GraphNode GraphNode
        {
            get { return node; }
        }

        public uint TentativeDistance { get; set; }

        public uint Penalty
        {
            get { return node.Penalty; }
        }

        public bool IsVisited { get; set; }

        private GraphNode node;

        public GraphNodeAdapter(GraphNode node)
        {
            this.node = node;
        }

        public List<IGraphNode> FindNeighbors(NNConstraint pathConstraint)
        {
            var neighbors = new List<IGraphNode>();

            node.GetConnections(graphNode =>
            {
                // Restrict the neighboring nodes by the path constraint.
                if (pathConstraint.Suitable(graphNode))
                {
                    neighbors.Add(new GraphNodeAdapter(graphNode));
                }
            });

            return neighbors;
        }

        public uint GetConnectionDistance(IGraphNode targetNode)
        {
            return targetNode.Penalty;
        }

        public IGraphNode Clone()
        {
            var node = new GraphNodeAdapter(GraphNode);
            node.IsVisited = IsVisited;
            node.TentativeDistance = TentativeDistance;
            return node;
        }
    }
}