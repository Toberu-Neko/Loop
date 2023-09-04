using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordAttackState : PlayerAttackState
{
    public PlayerSwordAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.Anim.speed = PlayerInventoryManager.Instance.SwordMultiplier.attackSpeedMultiplier;
    }

    public override void Exit()
    {
        base.Exit();
        player.Anim.speed = 1f;
    }
}
