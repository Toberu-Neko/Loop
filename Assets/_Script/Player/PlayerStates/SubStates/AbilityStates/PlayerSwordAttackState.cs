using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttackState : PlayerAbilityState
{
    [SerializeField] private float holdAttackTime = 2f;
    
    private bool holdAttackInput;
    private int xInput;

    private bool holdEnough;
    protected int attackCounter = 0;

    private bool canAttack;
    public PlayerSwordAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseAttackInput();
        holdEnough = false;
        canAttack = false;
    }

    public override void Exit()
    {
        base.Exit();

        player.Anim.SetBool("swordHoldAttack", false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        holdAttackInput = player.InputHandler.HoldAttackInput;
        xInput = player.InputHandler.NormInputX;

        if (!holdEnough && Time.time >= startTime + holdAttackTime)
        {
            player.Anim.SetBool("swordHoldAttack", true);
            holdEnough = true;
        }
        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }

        Movement.CheckIfShouldFlip(xInput);

        if (!CollisionSenses.Ground)
        {
            stateMachine.ChangeState(player.SwordSkyAttackState);
        }
        else if (!holdAttackInput && Time.time < startTime + holdAttackTime) 
        {
            stateMachine.ChangeState(player.SwordNormalAttackState);
        }
        else if (!holdAttackInput && Time.time >= startTime + holdAttackTime)
        {
            stateMachine.ChangeState(player.SwordStrongAttackState);
        }

    }
    public bool CheckIfCanAttack() => canAttack;
    public void ResetCanAttack() => canAttack = true;
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
