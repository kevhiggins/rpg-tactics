using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.PathFinding
{
    class PathConstraint : PathNNConstraint
    {
        public const int TagBasicGround = 1 << 0;
        public const int TagHasUnit = 1 << 1;
        public const int TagSourceUnit = 1 << 2;
        public const int TagTargetUnit = 1 << 3;
        public const int TagNone = 0;

        public List<GraphNode> alwaysSuitableNodes = new List<GraphNode>();

        // The tags to use after the start and end nodes are found.
        public int afterEndTags = -1;

        public PathConstraint()
        {
            constrainArea = true;
        }

        public static PathConstraint OnlyEmptySpaces
        {
            get
            {
                var n = new PathConstraint();
                n.constrainArea = true;
                n.tags = TagBasicGround;
                return n;
            }
        }

        public override bool Suitable(GraphNode node)
        {
            if (alwaysSuitableNodes.Contains(node))
                return true;
            return base.Suitable(node);
        }
    }
}