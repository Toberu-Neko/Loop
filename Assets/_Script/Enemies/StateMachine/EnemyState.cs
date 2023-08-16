using System.Collections;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Entity entity;
    protected Core core;


    protected bool isAnimationFinished;
    public float StartTime { get; protected set;}

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
    }

    public virtual void Enter()
    {
        StartTime = Time.time;
        entity.Anim.SetBool(animBoolName, true);

        DoChecks();
    }
    public virtual void Exit()
    {
        entity.Anim.SetBool(animBoolName, false);
    }
    public virtual void AnimationFinishTrigger()
    {
        isAnimationFinished = true;
    }

    public virtual void LogicUpdate()
    {
        if (Stats.IsTimeStopped)
        {
            StartTime += Time.deltaTime;
            Movement.SetVelocityZero();
            return;
        }

        if (Stats.IsTimeSlowed)
        {
            StartTime += Time.deltaTime * (1f - GameManager.Instance.TimeSlowMultiplier);
        }
    }

    public float Timer(float timer)
    {
        if (Stats.IsTimeStopped)
        {
            timer += Time.deltaTime;
            Movement.SetVelocityZero();
            return timer;
        }

        if (Stats.IsTimeSlowed)
        {
            timer += Time.deltaTime * (1f - GameManager.Instance.TimeSlowMultiplier);
            return timer;
        }
        return timer;
    }

    public virtual void PhysicsUpdate(){ }
    public virtual void DoChecks(){ }
    public virtual void AnimationActionTrigger() { }
}
