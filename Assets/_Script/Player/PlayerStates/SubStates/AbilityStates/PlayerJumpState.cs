using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    public int AmountOfJumpsLeft { get; private set; }

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        if (player.NoHand)
        {
            AmountOfJumpsLeft = 1;
        }
        else
        {
            AmountOfJumpsLeft = playerData.amountOfJumps;
        }
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityY(playerData.jumpVelocity);
        AmountOfJumpsLeft--;
        player.InputHandler.UseJumpInput();
        player.InAirState.SetIsJumping();

        isAbilityDone = true;
        AudioManager.Instance.PlaySoundFX(player.PlayerSFX.jump, Movement.ParentTransform, AudioManager.SoundType.twoD);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityY(playerData.jumpVelocity);
    }

    public bool CanJump()
    {
        if(AmountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetAmountOfJumpsLeft()
    {
        if(player.NoHand)
        {
            AmountOfJumpsLeft = 1;
        }
        else
        {
            AmountOfJumpsLeft = playerData.amountOfJumps;
        }
    }

    public void DecreaseAmountOfJumpsLeft() => AmountOfJumpsLeft--;
}
