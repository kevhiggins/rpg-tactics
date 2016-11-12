using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rpg.GameState.Behaviors
{
    class EndAction : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var winningTeamId = WinningTeam();
            if (winningTeamId != 0)
            {
                SceneManager.LoadScene("Scenes/End");
            }

            var hasActions = !ActiveUnit.HasActed || !ActiveUnit.HasMoved;
            animator.SetBool("Has Actions", !ActiveUnit.HasActed || !ActiveUnit.HasMoved);
            if (!hasActions)
            {
                ActiveUnit.EndTurn();
            }

            animator.SetTrigger("State Complete");
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        protected int WinningTeam()
        {
            var actionQueue = GameManager.instance.actionQueue;
            var livingTeams = new HashSet<int>();

            foreach (var tmpUnit in actionQueue.UnitList)
            {
                if (!tmpUnit.IsDead)
                {
                    livingTeams.Add(tmpUnit.TeamId);
                }
            }

            if (livingTeams.Count == 1)
            {
                return livingTeams.First();
            }
            return 0;
        }
    }
}
