using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.PathFinding
{
    class ABPathExclusion : ABPath
    {
        public List<GraphNode> alwaysTraversableNodes = new List<GraphNode>();

        public new static ABPath Construct(Vector3 start, Vector3 end, OnPathDelegate callback = null)
        {
            var p = PathPool.GetPath<ABPathExclusion>();

            p.Setup(start, end, callback);
            return p;
        }

        public override bool CanTraverse(GraphNode node)
        {
            if (alwaysTraversableNodes.Contains(node))
                return true;
            return base.CanTraverse(node);
        }

        protected new void Setup(Vector3 start, Vector3 end, OnPathDelegate callbackDelegate)
        {
            base.Setup(start, end, callbackDelegate);
        }
    }
}
