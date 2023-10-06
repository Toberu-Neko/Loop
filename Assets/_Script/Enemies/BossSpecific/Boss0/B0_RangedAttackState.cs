using UnityEngine;

public class B0_RangedAttackState : SingleRangedAttackState
{
    private Boss0 boss;
    public B0_RangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyRangedAttackState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
