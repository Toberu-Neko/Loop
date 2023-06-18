using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordNormalAttackState : PlayerAbilityState
{
    public PlayerSwordNormalAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        
        Debug.Log("Do Sword Normal Attack");
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAbilityDone = true;
    }

}
