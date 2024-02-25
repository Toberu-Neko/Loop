using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnOnState : PlayerAbilityState
{
    public PlayerTurnOnState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySoundFX(player.PlayerSFX.turnOn, player.transform, AudioManager.SoundType.twoD);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        stateMachine.ChangeState(player.IdleState);
    }
}
