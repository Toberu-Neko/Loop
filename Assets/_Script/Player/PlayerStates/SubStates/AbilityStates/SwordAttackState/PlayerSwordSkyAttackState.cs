using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordSkyAttackState : PlayerAbilityState
{
    public PlayerSwordSkyAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        Combat.OnDamaged += () => isAbilityDone = true;
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

        DoDamageToDamageList(20, new Vector2(3, 1), 20);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

}
