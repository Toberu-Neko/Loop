using UnityEngine;

public class EnemyFlyingIdleState : EnemyState
{
    protected ED_EnemyIdleState stateData;

    protected bool gotoNextState;

    private float idleTime;
    public EnemyFlyingIdleState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyIdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetRBKinematic();
        Movement.SetVelocityZero();
        gotoNextState = false;
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();

        if (Time.time >= StartTime + idleTime)
        {
            gotoNextState = true;
        }
    }
}
