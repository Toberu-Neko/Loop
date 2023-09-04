using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttackState : PlayerAttackState
{
    public PlayerGunAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.Anim.speed = PlayerInventoryManager.Instance.GunMultiplier.attackSpeedMultiplier;
    }

    public override void Exit()
    {
        base.Exit();
        player.Anim.speed = 1f;
    }
}
