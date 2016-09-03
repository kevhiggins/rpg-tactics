using System.Collections.Generic;

namespace GraphPathfinding
{
    public interface IGraphNode
    {
        int Id { get; }
        int X { get; }
        int Y { get; }

        List<IGraphNode> FindNeighbors();

        IGraphNode ParentNode { get; set; }
        int TentativeCost { get; set; }
    }
}
