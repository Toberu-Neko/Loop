using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnhanceState : PlayerSwordAttackState
{
    public SwordEnhanceState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Exit()
    {
        base.Exit();

        player.WeaponManager.SetEnhanceSwordAttack(true);
        player.WeaponManager.DecreaseEnergy();
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

        isAttackDone = true;
    }
}
