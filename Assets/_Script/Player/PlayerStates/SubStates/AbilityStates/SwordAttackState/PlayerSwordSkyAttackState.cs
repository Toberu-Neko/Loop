using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkyAttackState : PlayerAbilityState
{
    private WeaponAttackDetails details;
    public PlayerSwordSkyAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        details = player.PlayerWeaponManager.SwordData.skyAttackDetails;
    }

    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += () => isAbilityDone = true;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= () => isAbilityDone = true;
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

        DoDamageToDamageList(details.damageAmount, details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

}
