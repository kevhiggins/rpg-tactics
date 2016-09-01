using System.Collections.Generic;
using Pathfinding;

namespace GraphPathfinding
{
    public interface IGraphNode
    {
        int Id { get; }

        int X { get; }
        int Y { get; }

        GraphNode GraphNode { get; }
        uint TentativeCost { get; set; }
        uint Penalty { get; }
        bool IsVisited { get; set; }
        List<IGraphNode> FindNeighbors(NNConstraint pathConstraint);
        uint GetConnectionDistance(IGraphNode targetNode);

        IGraphNode Clone();
    }
}
