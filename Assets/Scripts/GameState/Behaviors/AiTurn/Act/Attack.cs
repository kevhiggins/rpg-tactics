using UnityEngine;

namespace Rpg.GameState.Behaviors.AiTurn.Act
{
    class Attack : AbstractActiveUnitStateBehavior
    {
        public override void Enable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var targetTile = GameManager.instance.UnitTurn.ActTargetTile;
            GameManager.instance.battleManager.AttackUnit(ActiveUnit, targetTile.GetUnit(), () => { animator.SetTrigger("State Complete"); });
        }

        public override void Disable(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public override void StateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}