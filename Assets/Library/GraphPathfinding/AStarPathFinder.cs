using System;

namespace GraphPathfinding
{
    public delegate uint MovementCostFunction(IGraphNode node);
    public delegate uint CostFunction(IGraphNode node);
    public delegate uint HeuristicFunction(IGraphNode node, IGraphNode goalNode);

    // TODO decouple this from Unity A* pathfinder
    public class AStarPathfinder
    {
        protected MovementCostFunction movementCost;
        protected CostFunction cost;
        protected HeuristicFunction heuristic;

        /// <summary>
        /// Create pathfinder class with default functionality.
        /// </summary>
        public AStarPathfinder() : this(null, null, null)
        {
        }

        /// <summary>
        /// Configure the cost functions and heuristic to use a different implementation.
        /// </summary>
        /// <param name="movementCostFunction"></param>
        /// <param name="costFunction"></param>
        /// <param name="heuristicFunction"></param>
        public AStarPathfinder(MovementCostFunction movementCostFunction, CostFunction costFunction, HeuristicFunction heuristicFunction)
        {
            // If specified, use the override functions. Otherwise, use their defaults.
            if (movementCostFunction == null)
                movementCost = MovementCost;
            if (costFunction == null)
                cost = Cost;
            if(heuristicFunction == null)
                heuristic = ManhattanDistanceHeuristic;
        }

        protected uint MovementCost(IGraphNode node)
        {
            return 1;
        }

        protected uint Cost(IGraphNode node)
        {
            return node.TentativeCost;
        }

        protected uint ManhattanDistanceHeuristic(IGraphNode node, IGraphNode goalNode)
        {
            var dx = Math.Abs(node.X - goalNode.X);
            var dy = Math.Abs(node.Y - goalNode.Y);
            return 1;
        }
    }
}
