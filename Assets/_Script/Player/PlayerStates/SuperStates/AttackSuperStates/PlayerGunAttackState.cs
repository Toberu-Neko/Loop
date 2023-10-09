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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.speed = PlayerInventoryManager.Instance.GunMultiplier.attackSpeedMultiplier * Stats.AnimationSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }
}
