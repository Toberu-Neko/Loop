using UnityEngine;

public class B0_PlayerDetectedState : PlayerDetectedState
{
    private Boss0 boss;
    public B0_PlayerDetectedState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyPlayerDetectedState stateData, Boss0 boss) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
