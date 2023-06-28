using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerAttackState
{
    private bool blockInput;
    private int xInput;

    private float lastBlockTime;

    private bool knockbackFinished;
    private bool damageFinished;
    private bool perfectBlock;

    public PlayerBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += DamageFinished;
        Combat.OnPerfectBlock += PerfectBlock;
        Combat.OnKnockback += KnockbackFinished;

        Combat.PerfectBlock = true;
        Combat.NormalBlock = true;

        knockbackFinished = false;
        damageFinished = false;
        perfectBlock = false;

        lastBlockTime = 0;
    }

    public override void Exit()
    {
        base.Exit();

        lastBlockTime = Time.time;

        Combat.PerfectBlock = false;
        Combat.NormalBlock = false;

        Combat.OnDamaged -= DamageFinished;
        Combat.OnPerfectBlock -= PerfectBlock;
        Combat.OnKnockback -= KnockbackFinished;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        blockInput = player.InputHandler.BlockInput;

        Movement.CheckIfShouldFlip(xInput);
        Movement.SetVelocityX(playerData.blockMovementVelocity * xInput);

        if(Time.time >= startTime + playerData.perfectBlockTime && Combat.PerfectBlock)
        {
            Combat.PerfectBlock = false;
        }

        if (!isExitingState)
        {
            if (perfectBlock)
            {
                stateMachine.ChangeState(player.PerfectBlockState);
            }
            else if (!blockInput || (knockbackFinished && damageFinished))
            {
                isAbilityDone = true;
            }
        }
    }
    private void GoToPerfectBlockState() => stateMachine.ChangeState(player.PerfectBlockState);

    private void KnockbackFinished() => knockbackFinished = true;
    
    private void DamageFinished() => damageFinished = true;

    private void PerfectBlock() => perfectBlock = true;

    public bool CheckIfCanBlock()
    {
        return Time.time >= lastBlockTime + playerData.blockCooldown;
    }
}
