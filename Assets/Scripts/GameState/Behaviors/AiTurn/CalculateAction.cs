using System.Collections.Generic;
using Rpg.Widgets;
using UnityEngine;
using Rpg.Map;
using GraphPathfinding;
using System.Linq;
using Rpg.PathFinding;

namespace Rpg.GameState.Behaviors.AiTurn
{
    class CalculateAction : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameManager.instance.levelManager.GetMap().SetTileCursor(ActiveUnit.GetTile().tilePosition, () =>
            {
                RegisterWidget(new UnitInfoWidget(ActiveUnit));

                // Determine the target positions from all enemy units.
                var targetPositions = new List<TilePosition>();
                var enemyUnits = GameManager.instance.actionQueue.GetEnemyUnits(ActiveUnit.TeamId);

                // If there are no enemy units, have the current unit wait.
                if (enemyUnits.Count < 1)
                {
                    animator.SetTrigger("Wait");
                    return;
                }

                // Otherwise, find the nearest path to a target, and move/attack appropriately.
                foreach (var enemyUnit in enemyUnits)
                {
                    targetPositions.Add(enemyUnit.GetTile().tilePosition);
                }

                var shortestPath = GameManager.instance.PathManager.FindNearestTargetPath(ActiveUnit.GetTile().tilePosition, targetPositions);
                ProcessNearestEnemyPath(shortestPath, animator);
            });
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        private void ProcessNearestEnemyPath(Path shortestPath, Animator animator)
        {
            // Remove the first position, since it is the unit's current location.
            if(shortestPath != null && shortestPath.nodes.Any())
                shortestPath.nodes.RemoveAt(0);

            // If there are no nodes other than the sourceUnit node, then we should wait, since the unit is trapped, or has 0 movement.
            if (shortestPath == null || !shortestPath.nodes.Any())
            {
                animator.SetTrigger("Wait");
                return;
            }

            // Remove the last position, because we only want to move the enemy next to the target.
            var targetIndex = shortestPath.nodes.Count - 1;
            var targetNode = (GraphNodeTile)shortestPath.nodes[targetIndex];
            shortestPath.nodes.RemoveAt(targetIndex);

            // If the unit is already where it wants to be, and has not acted then perform an action.
            if (!shortestPath.nodes.Any() && ActiveUnit.HasActed == false)
            {
                // Get the target tile's unit and attack it.
                var unitTile = targetNode.Tile;

                GameManager.instance.UnitTurn.ActTargetTile = unitTile;
                animator.SetTrigger("Act");
                return;
            }

            // If we have a unit in range, but have already acted and moved, then wait.
            if (!shortestPath.nodes.Any() || ActiveUnit.HasMoved)
            {
                animator.SetTrigger("Wait");
                return;
            }

            var nodes = shortestPath.nodes;
            var distance = 0;
            var movePath = new List<GraphNodeTile>();

            // TODO we need a way to determine the costs


            // TODO FIX THIS DOESNT WORK ANYMORE
            foreach (GraphNodeTile node in nodes)
            {
                // Add one, since the distance between each tile is 1 unit.
                distance++;
              
                // Add the node penalty
                distance += node.Tile.Penalty;
                
                // If the distance traveled after adding this node exceeds the unit move speed, than don't add the node to the list, and break out of loop.
                if (distance > ActiveUnit.MovementSpeed)
                    break;

                movePath.Add(node);
            }

            // Set the movement path on the unit turn, and progress to the next state.
            GameManager.instance.UnitTurn.MovementPath = movePath;
            animator.SetTrigger("Move");
        }
    }
}
