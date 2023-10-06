using UnityEngine;

public class E2_RangedAttackState : SingleRangedAttackState
{
    private Enemy2 enemy;
    public E2_RangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyRangedAttackState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if(isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }
}
