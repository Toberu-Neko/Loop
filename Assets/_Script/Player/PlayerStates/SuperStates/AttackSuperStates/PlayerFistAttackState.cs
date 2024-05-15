using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFistAttackState : PlayerAttackState
{
    public PlayerFistAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.speed = PlayerInventoryManager.Instance.FistMultiplier.attackSpeedMultiplier * Stats.AnimationSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void AnimationSFXTrigger()
    {
        base.AnimationSFXTrigger();
        AudioManager.Instance.PlayRandomSoundFX(player.PlayerSFX.swordAttack, Movement.ParentTransform);
    }
}
