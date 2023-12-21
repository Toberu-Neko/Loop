using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttackState : PlayerAttackState
{
    protected SO_WeaponData_Sword swordData;
    public PlayerSwordAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        swordData = player.WeaponManager.SwordData;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.speed = PlayerInventoryManager.Instance.SwordMultiplier.attackSpeedMultiplier * Stats.AnimationSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void AnimationSFXTrigger()
    {
        base.AnimationSFXTrigger();
        AudioManager.instance.PlayRandomSoundFX(player.PlayerSFX.swordAttack, Movement.ParentTransform);
    }
}
