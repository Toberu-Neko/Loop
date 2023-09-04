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
        holdAttackTime = player.PlayerWeaponManager.SwordData.strongAttackHoldTime;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += () => isAttackDone = true;
        player.InputHandler.UseAttackInput();
        holdEnough = false;
        canAttack = false;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= () => isAttackDone = true;
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
    public void SetCanAttackFalse() => canAttack = false;
    public void ResetCanAttack() => canAttack = true;
}
