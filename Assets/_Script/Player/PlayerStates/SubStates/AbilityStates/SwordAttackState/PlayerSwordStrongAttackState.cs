using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordStrongAttackState : PlayerAbilityState
{
    public PlayerSwordStrongAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(25, new Vector2(3, 1), 25);

    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
    }
}
