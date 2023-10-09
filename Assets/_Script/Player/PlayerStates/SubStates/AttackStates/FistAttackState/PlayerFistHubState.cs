using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistHubState : PlayerFistAttackState
{
    private SO_WeaponData_Fist data;
    private bool canAttack;

    private int xInput;
    private int yInput;
    private bool holdAttackInput;

    private float strongAttackHoldTime;

    private int chargeStage;
    
    public PlayerFistHubState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.WeaponManager.FistData;
        strongAttackHoldTime = data.strongAttackHoldTime;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnKnockback += () => isAttackDone = true;
        player.InputHandler.UseAttackInput();
        canAttack = false;
        player.Anim.SetInteger("fistHubChargeStage", chargeStage);
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnKnockback -= () => isAttackDone = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        holdAttackInput = player.InputHandler.HoldAttackInput;
        xInput = player.InputHandler.NormInputX;
        yInput = player.InputHandler.NormInputY;

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }

        Movement.CheckIfShouldFlip(xInput);

        if(!holdAttackInput && Time.time < StartTime + strongAttackHoldTime)
        {
            stateMachine.ChangeState(player.FistNormalAttackState);
        }
        else if (!holdAttackInput && yInput == 0f && Time.time >= StartTime + strongAttackHoldTime)
        {
            stateMachine.ChangeState(player.FistStrongAttackState);
        }
        else if(holdAttackInput && yInput < 0f && Time.time >= StartTime + strongAttackHoldTime)
        {
            stateMachine.ChangeState(player.FistStaticStrongAttackState);
        }
    }

    public bool CheckIfCanAttack() => canAttack;
    public void SetCanAttackFalse() => canAttack = false;
    public void ResetCanAttack() => canAttack = true;
}
