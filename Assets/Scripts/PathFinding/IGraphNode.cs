using System.Collections.Generic;

namespace Rpg.PathFinding
{
    public interface IGraphNode
    {
        int Id { get; }
        float TentativeDistance { get; set; }
        bool IsVisited { get; set; }
        List<IGraphNode> FindUnvisitedNeighbors();
        float GetConnectionDistance(IGraphNode targetNode);
    }
}
