using UnityEngine;

public class B0N_NormalAttackState2 : SingleMeleeAttackState
{
    private Boss0New boss;
    public B0N_NormalAttackState2(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyMeleeAttackState stateData, Boss0New boss) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        AudioManager.instance.PlaySoundFX(boss.StateData.normalAttackSFX, Movement.ParentTransform, AudioManager.SoundType.threeD);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        boss.NormalAttackState1.SetEndTime(EndTime);
        stateMachine.ChangeState(boss.PlayerDetectedMoveState);
    }
}
