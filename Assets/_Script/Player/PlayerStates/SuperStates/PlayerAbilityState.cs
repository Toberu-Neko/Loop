using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;

    //If have an abality that needs to know if grounded, change to protect.
    private bool isGrounded;

    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;
    private CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>(); }
    private CollisionSenses collisionSenses;

    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(CollisionSenses)
            isGrounded = CollisionSenses.Ground;

    }

    public override void Enter()
    {
        base.Enter();
        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAbilityDone)
        {
            if(isGrounded && Movement?.CurrentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
