﻿using System.Collections.Generic;
using Pathfinding;

namespace Rpg.PathFinding
{
    public interface IGraphNode
    {
        int Id { get; }
        GraphNode GraphNode { get; }
        uint TentativeDistance { get; set; }
        uint Penalty { get; }
        bool IsVisited { get; set; }
        List<IGraphNode> FindNeighbors(NNConstraint pathConstraint);
        uint GetConnectionDistance(IGraphNode targetNode);

        IGraphNode Clone();
    }
}
