using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreBlockState : PlayerAttackState
{
    private float lastBlockTime;
    private bool perfectBlock;
    public PlayerPreBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnPerfectBlock += PerfectBlock;

        Combat.SetPerfectBlock(true);
        Combat.SetNormalBlock(true);

        perfectBlock = false;

        lastBlockTime = 0f;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
        else
        {
            Movement.SetVelocityX(0f);
        }

        if (!isExitingState)
        {
            if (perfectBlock)
            {
                lastBlockTime = 0f;
                CamManager.Instance.CameraShake();
                Combat.SetPerfectBlock(false);
                Combat.SetNormalBlock(false);
                stateMachine.ChangeState(player.PerfectBlockState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnPerfectBlock -= PerfectBlock;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (player.InputHandler.BlockInput)
        {
            stateMachine.ChangeState(player.BlockState);
        }
        else
        {
            Combat.SetPerfectBlock(false);
            Combat.SetNormalBlock(false);
            isAttackDone = true;
        }
    }

    private void PerfectBlock() => perfectBlock = true;

    public bool CheckIfCanBlock()
    {
        return Time.time >= lastBlockTime + playerData.blockCooldown;
    }

    public void SetLastBlockTime(float time) => lastBlockTime = time;
}
