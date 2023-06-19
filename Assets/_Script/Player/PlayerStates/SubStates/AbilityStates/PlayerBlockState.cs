using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockState : PlayerAbilityState
{
    private bool blockInput;
    private int xInput;

    private float lastBlockTime;

    public PlayerBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        Combat.OnDamaged += DamagedWhileBlocking;
        Combat.OnPerfectBlock += GoToPerfectBlockState;

        Combat.PerfectBlock = true;
        Combat.NormalBlock = true;

        lastBlockTime = 0;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= DamagedWhileBlocking;
        Combat.OnPerfectBlock -= GoToPerfectBlockState;

        Combat.PerfectBlock = false;
        Combat.NormalBlock = false;
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
            if (!blockInput)
            {
                Combat.PerfectBlock = false;
                Combat.NormalBlock = false;
                isAbilityDone = true;
            }
        }
    }
    private void GoToPerfectBlockState() => stateMachine.ChangeState(player.PerfectBlockState);
    private void DamagedWhileBlocking()
    {
        lastBlockTime = Time.time;
        isAbilityDone = true;
    }

    public bool CheckIfCanBlock()
    {
        return Time.time >= lastBlockTime + playerData.blockCooldown;
    }
}
