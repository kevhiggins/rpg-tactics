using System.Collections.Generic;
using Rpg.Widgets;
using UnityEngine;

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
                var targetPositions = new List<Vector3>();
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
                    targetPositions.Add(enemyUnit.GetGameObject().transform.position);
                }
                var activeUnitPosition = ActiveUnit.GetGameObject().transform.position;
                GameManager.instance.PathManager.FindNearestTargetPath(activeUnitPosition, targetPositions, path => ProcessNearestEnemyPath(path, animator));
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
            var map = GameManager.instance.levelManager.GetMap();
            
            // Remove the first position, since it is the unit's current location.
            if(shortestPath.vectorPath.Any())
                shortestPath.vectorPath.RemoveAt(0);

            // If there are no nodes other than the sourceUnit node, then we should wait, since the unit is trapped, or has 0 movement.
            if (shortestPath.vectorPath.Any() == false)
            {
                animator.SetTrigger("Wait");
                return;
            }

            // Remove the last position, because we only want to move the enemy next to the target.
            var targetIndex = shortestPath.vectorPath.Count - 1;
            var targetPosition = shortestPath.vectorPath[targetIndex];
            shortestPath.vectorPath.RemoveAt(targetIndex);

            // If the unit is already where it wants to be, and has not acted then perform an action.
            if (shortestPath.vectorPath.Count == 0 && ActiveUnit.HasActed == false)
            {
                // Get the target tile's unit and attack it.
                var unitTile = map.FindTileAtPosition(targetPosition);

                GameManager.instance.UnitTurn.ActTargetTile = unitTile;
                animator.SetTrigger("Act");
                return;
            }

            // If we have a unit in range, but have already acted, then wait.
            if (shortestPath.vectorPath.Count == 0 || ActiveUnit.HasMoved)
            {
                animator.SetTrigger("Wait");
                return;
            }

            // Remove the final path node, since that's where the target is.
            var nodePositionList = shortestPath.vectorPath;
            var distance = 0;
            var movePath = new List<Vector3>();

            // TODO FIX THIS DOESNT WORK ANYMORE
            foreach (var nodePosition in nodePositionList)
            {
                var node = AstarPath.active.GetNearest(nodePosition).node;
                distance += (int)node.Penalty;
                if (distance > ActiveUnit.MovementSpeed)
                    break;
                movePath.Add(nodePosition);
            }

            // Set the movement path on the unit turn, and progress to the next state.
            GameManager.instance.UnitTurn.MovementPath = movePath;
            animator.SetTrigger("Move");
        }
    }
}
