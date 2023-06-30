using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;
    protected Core core;


    protected bool isAnimationFinished;
    public float StartTime { get; protected set;}

    protected string animBoolName;

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName)
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

    public virtual void LogicUpdate(){ }
    public virtual void PhysicsUpdate(){ }
    public virtual void DoChecks(){ }
    public virtual void AnimationActionTrigger() { }
}
