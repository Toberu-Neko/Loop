using UnityEngine;

public class KinematicState : EnemyState
{
    private float timer = -1f;
    protected bool gotoStunState;
    public KinematicState(Entity entity, EnemyStateMachine stateMachine, string animBoolName) : base(entity, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        gotoStunState = false;
        Movement.SetRBKinematic();
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
        timer = -1f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= StartTime + timer && timer != -1f)
        {
            gotoStunState = true;
        }
    }

    public void SetTimer(float time)
    {
        timer = time;
    }

    public void SetGotoStunStateTrue()
    {
        gotoStunState = true;
    }
}
