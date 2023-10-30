using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Entity entity;
    protected Core core;

    protected bool isAnimationFinished;
    protected bool isAnimationStartMovement;

    public float StartTime { get; protected set;}
    public float EndTime { get; protected set;}
    protected string animBoolName;

    protected Stats Stats { get; private set; }
    protected CollisionSenses CollisionSenses { get; private set; }
    protected Combat Combat { get; private set; }
    protected Movement Movement { get; private set; }
    protected Death Death { get; private set; }
    protected CheckPlayerSenses CheckPlayerSenses { get; private set; }

    public EnemyState(Entity entity, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        core = entity.Core;
        CheckPlayerSenses = core.GetCoreComponent<CheckPlayerSenses>();
        Death = core.GetCoreComponent<Death>();
        Movement = core.GetCoreComponent<Movement>();
        Combat = core.GetCoreComponent<Combat>();
        CollisionSenses = core.GetCoreComponent<CollisionSenses>();
        Stats = core.GetCoreComponent<Stats>();

        StartTime = 0f;
        EndTime = 0f;
    }

    public virtual void Enter()
    {
        StartTime = Time.time;
        entity.Anim.SetBool(animBoolName, true);

        DoChecks();
    }
    public virtual void Exit()
    {
        EndTime = Time.time;
        entity.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {
        StartTime = Stats.Timer(StartTime);
        EndTime = Stats.Timer(EndTime);

        if (Stats.IsTimeStopped)
        {
            Movement.SetVelocityZero();
            return;
        }
    }

    public virtual void PhysicsUpdate(){ }
    public virtual void DoChecks(){ }
    public virtual void AnimationActionTrigger() { }
    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
    }
    public virtual void AnimationStartMovementTrigger() { isAnimationStartMovement = true; }
    public virtual void AnimationStopMovementTrigger() { isAnimationStartMovement = false; }

    public virtual void Disable()
    {
        StartTime = 0f;
        EndTime = 0f;
    }


    public float ReturnHealthPercentage()
    {
        return Stats.Health.CurrentValue / Stats.Health.MaxValue;
    }

    public void SetEndTime(float value)
    {
        EndTime = value;
    }
}
