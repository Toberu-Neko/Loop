using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordHubState : PlayerSwordAttackState
{
    private float holdAttackTime;
    
    private int xInput;
    private bool holdAttackInput;

    private bool holdEnough;
    protected int attackCounter = 0;

    private bool canAttack;
    public PlayerSwordHubState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        holdAttackTime = player.WeaponManager.SwordData.strongAttackHoldTime;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnKnockback += () => isAttackDone = true;
        player.InputHandler.UseAttackInput();
        holdEnough = false;
        canAttack = false;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnKnockback -= () => isAttackDone = true;
        player.Anim.SetBool("swordHoldAttack", false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        holdAttackInput = player.InputHandler.HoldAttackInput;
        xInput = player.InputHandler.NormInputX;

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }

        Movement.CheckIfShouldFlip(xInput);

        if (Stats.CounterAttackable)
        {
            Stats.SetPerfectBlockAttackFalse();
            stateMachine.ChangeState(player.SwordCounterAttackState);
        }
        else if (!CollisionSenses.Ground)
        {
            stateMachine.ChangeState(player.SwordSkyAttackState);
        }
        else if (!holdAttackInput && !holdEnough && !player.WeaponManager.EnhanceSwordAttack) 
        {
            stateMachine.ChangeState(player.SwordNormalAttackState);
        }
        else if (!holdAttackInput && !holdEnough && player.WeaponManager.EnhanceSwordAttack)
        {
            stateMachine.ChangeState(player.SwordEnhancedAttackState);
        }
        else if (!holdAttackInput && holdEnough)
        {
            stateMachine.ChangeState(player.SwordStrongAttackState);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();


        player.Anim.SetBool("swordHoldAttack", true);
        holdEnough = true;
    }

    public bool CheckIfCanAttack() => canAttack && Stats.Attackable;
    public void SetCanAttackFalse() => canAttack = false;
    public void ResetCanAttack() => canAttack = true;
}
