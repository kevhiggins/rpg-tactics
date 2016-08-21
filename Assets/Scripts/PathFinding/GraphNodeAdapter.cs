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

        public List<IGraphNode> FindNeighbors()
        {
            var neighbors = new List<IGraphNode>();

            node.GetConnections(graphNode => { neighbors.Add(new GraphNodeAdapter(graphNode)); });

            return neighbors;
        }

        public uint GetConnectionDistance(IGraphNode targetNode)
        {
            return targetNode.Penalty;
        }
    }
}