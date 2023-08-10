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

        Combat.SetPerfectBlock(true);
        Combat.SetNormalBlock(true);

        knockbackFinished = false;
        damageFinished = false;
        perfectBlock = false;

        lastBlockTime = 0;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.SetPerfectBlock(false);
        Combat.SetNormalBlock(false);

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
        Movement.SetVelocityX(playerData.movementVelocity * playerData.blockMovementMultiplier * xInput);

        if(Time.time >= startTime + playerData.perfectBlockTime && Combat.PerfectBlock)
        {
            Combat.SetPerfectBlock(false);
        }

        if (!isExitingState)
        {
            if (perfectBlock)
            {
                lastBlockTime = Time.time;
                CamManager.instance.CameraShake();
                stateMachine.ChangeState(player.PerfectBlockState);
            }
            else if(knockbackFinished && damageFinished)
            {
                lastBlockTime = Time.time;
                isAttackDone = true;
            }
            else if (!blockInput)
            {
                lastBlockTime = 0;
                isAttackDone = true;
            }
        }
    }

    private void KnockbackFinished() => knockbackFinished = true;
    
    private void DamageFinished() => damageFinished = true;

    private void PerfectBlock() => perfectBlock = true;

    public bool CheckIfCanBlock()
    {
        return Time.time >= lastBlockTime + playerData.blockCooldown;
    }
}
