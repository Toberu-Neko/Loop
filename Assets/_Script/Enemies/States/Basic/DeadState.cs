public class DeadState : EnemyState
{

    public DeadState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        Movement.SetCanSetVelocity(false);
        Stats.SetInvincibleTrue();
        entity.gameObject.layer = 15;

        if (CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }

    public override void Exit()
    {
        base.Exit();

        Stats.SetInvincibleFalse();
        Movement.SetCanSetVelocity(true);
        entity.gameObject.layer = 13;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(CollisionSenses.Ground)
            Movement.SetVelocityZero();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
