using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkyAttackState : PlayerSwordAttackState
{
    private WeaponAttackDetails details;
    public PlayerSwordSkyAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        details = player.WeaponManager.SwordData.skyAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += () => isAttackDone = true;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= () => isAttackDone = true;
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

        DoDamageToDamageList(WeaponType.Sword, details);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

}
