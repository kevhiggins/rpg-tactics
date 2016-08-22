using Pathfinding;
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

        /** Called after the start node has been found. This is used to get different search logic for the start and end nodes in a path */

        public override void SetStart(GraphNode node)
        {
        }

        public override void SetEnd(GraphNode node)
        {
            Debug.Log("TEST!!!");
            // Set to unwalkable, since we initially set the node to walkable to get around library constraints.
//            if (afterEndTags != -1)
//                tags = afterEndTags;
            base.SetStart(node);
        }
    }
}