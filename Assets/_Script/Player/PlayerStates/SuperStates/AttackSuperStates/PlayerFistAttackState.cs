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
        player.Anim.speed = PlayerInventoryManager.Instance.FistMultiplier.attackSpeedMultiplier;
    }

    public override void Exit()
    {
        base.Exit();
        player.Anim.speed = 1f;
    }
}
