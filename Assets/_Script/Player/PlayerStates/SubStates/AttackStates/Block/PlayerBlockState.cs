using UnityEngine;

public class PlayerBlockState : PlayerAttackState
{
    private bool blockInput;
    private int xInput;

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

        if(Time.time >= StartTime + playerData.perfectBlockTime && Combat.PerfectBlock)
        {
            Combat.SetPerfectBlock(false);
        }

        if (!isExitingState)
        {
            if (perfectBlock)
            {
                player.PreBlockState.SetLastBlockTime(0f);
                CamManager.Instance.CameraShake(2f);
                stateMachine.ChangeState(player.PerfectBlockState);
            }
            else if(knockbackFinished && damageFinished)
            {
                player.InputHandler.UseBlockInput();
                player.PreBlockState.SetLastBlockTime(Time.time);
                isAttackDone = true;
            }
            else if (!blockInput)
            {
                player.PreBlockState.SetLastBlockTime(Time.time);
                isAttackDone = true;
            }
        }
    }

    private void KnockbackFinished() => knockbackFinished = true;
    
    private void DamageFinished() => damageFinished = true;

    private void PerfectBlock() => perfectBlock = true;

}
