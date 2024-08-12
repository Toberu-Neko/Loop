using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core core;
    protected Combat Combat;
    protected Stats Stats;
    protected Movement Movement;
    protected CollisionSenses CollisionSenses;

    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;
    protected bool isAnimationStartMovement;
    protected bool isExitingState;

    /// <summary>
    /// Time when the state is entered.
    /// </summary>
    protected float StartTime;

    private string animBoolName;
    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;

        core = player.Core;
        Combat = player.Core.GetCoreComponent<Combat>();
        Movement = player.Core.GetCoreComponent<Movement>();
        Stats = player.Core.GetCoreComponent<Stats>();
        CollisionSenses = player.Core.GetCoreComponent<CollisionSenses>();
        StartTime = 0f;
    }

    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    public virtual void Enter()
    {
        DoChecks();
        StartTime = Time.time;
        player.Anim.SetBool(animBoolName, true);
                
        isAnimationFinished = false;
        isExitingState = false;
        isAnimationStartMovement = false;
    }

    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    /// <summary>
    /// Called every Update, in player sript using Statemachine.CurrentState.LogicUpdate();.
    /// </summary>
    public virtual void LogicUpdate() 
    {
        player.Anim.speed = Stats.AnimationSpeed;
    }

    /// <summary>
    /// Called every fixedUpdate, in player sript using Statemachine.CurrentState.PhysicsUpdate();.
    /// </summary>
    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    /// <summary>
    /// Called every fixedUpdate, in player sript using Statemachine.CurrentState.DoChecks();.
    /// </summary>
    public virtual void DoChecks() { }

    public virtual void AnimationActionTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public virtual void AnimationStartMovementTrigger() { isAnimationStartMovement = true; }

    public virtual void AnimationStopMovementTrigger() { isAnimationStartMovement = false; Movement.SetVelocityZero(); }
    
    public virtual void AnimationTurnOnFlipTrigger() { }

    public virtual void AnimationTurnOffFlipTrigger() { }

    public virtual void EarlyFinishAnimation() { }

    public virtual void AnimationSFXTrigger() { }
}
