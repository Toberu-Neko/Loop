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

    protected Stats Stats => stats ? stats : core.GetCoreComponent<Stats>();
    private Stats stats;

    protected CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    protected Combat Combat => combat ? combat : core.GetCoreComponent<Combat>();
    private Combat combat;

    protected Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;

    protected Death Death => death ? death : core.GetCoreComponent<Death>();
    private Death death;

    protected CheckPlayerSenses CheckPlayerSenses => checkPlayerSenses ? checkPlayerSenses : core.GetCoreComponent<CheckPlayerSenses>();
    private CheckPlayerSenses checkPlayerSenses;


    public EnemyState(Entity entity, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        core = entity.Core;
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
        StartTime = Timer(StartTime);
        EndTime = Timer(EndTime);

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

    public float Timer(float timer)
    {
        if (Stats.IsTimeStopped)
        {
            timer += Time.deltaTime;
            return timer;
        }

        if (Stats.IsTimeSlowed)
        {
            timer += Time.deltaTime * (1f - GameManager.Instance.TimeSlowMultiplier);
            return timer;
        }
        return timer;
    }

    public float ReturnHealthPercentage()
    {
        return Stats.Health.CurrentValue / Stats.Health.MaxValue;
    }
}
