using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core core;
    protected Combat Combat;
    protected Stats Stats;
    protected Movement Movement;


    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;
    protected bool isAnimationStartMovement;
    protected bool isExitingState;

    protected float startTime;

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
    }
    public virtual void Enter()
    {
        DoChecks();
        startTime = Time.time;
        player.Anim.SetBool(animBoolName, true);
                
        isAnimationFinished = false;
        isExitingState = false;
        isAnimationStartMovement = false;
    }
    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }
    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public virtual void AnimationActionTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    public virtual void AnimationStartMovementTrigger() { isAnimationStartMovement = true; }

    public virtual void AnimationStopMovementTrigger() { isAnimationStartMovement = false; Movement.SetVelocityZero(); }
    
    public virtual void AnimationTurnOnFlipTrigger() { }

    public virtual void AnimationTurnOffFlipTrigger() { }
}
