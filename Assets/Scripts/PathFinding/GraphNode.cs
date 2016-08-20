using System;
using System.Collections.Generic;

namespace Rpg.PathFinding
{
    class GraphNode : IGraphNode
    {
        // TODO IMPLEMENT ID generation
        public int Id { get; private set; }
        public float TentativeDistance { get; set; }
        public bool IsVisited { get; set; }

        public List<IGraphNode> FindUnvisitedNeighbors()
        {
            throw new System.NotImplementedException();
        }

        public float GetConnectionDistance(IGraphNode targetNode)
        {
            throw new System.NotImplementedException();
        }
    }
}
