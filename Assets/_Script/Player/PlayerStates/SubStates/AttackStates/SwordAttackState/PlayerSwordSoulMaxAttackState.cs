using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSoulMaxAttackState : PlayerSwordAttackState
{
    WeaponAttackDetails[] details;
    private int count;
    public PlayerSwordSoulMaxAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        details = player.WeaponManager.SwordData.soulThreeAttackDetails;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(WeaponType.Sword, details[count], false);
        count++;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    public override void Enter()
    {
        base.Enter();

        player.WeaponManager.ClearCurrentEnergy();
        count = 0;

        Stats.SetInvincibleTrue();
    }

    public override void Exit()
    {
        base.Exit();

        Stats.SetInvincibleFalse();
    }
}
