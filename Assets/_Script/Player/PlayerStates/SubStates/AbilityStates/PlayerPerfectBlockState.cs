using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPerfectBlockState : PlayerAbilityState
{
    public PlayerPerfectBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        Collider2D[] enemy = Physics2D.OverlapCircleAll(player.transform.position, playerData.perfectBlockKnockbackRadius, playerData.whatIsEnemy);

        foreach (Collider2D enemyCollider in enemy)
        {
            int direction;
            if (enemyCollider.transform.position.x >= core.transform.position.x)
            {
                direction = 1;
            }
            else
                direction = -1;

            enemyCollider.GetComponentInChildren<IKnockbackable>().Knockback(playerData.perfectBlockKnockbackAngle ,playerData.perfectBlockKnockbackForce, direction);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        Combat.PerfectBlock = false;
        Combat.NormalBlock = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
            isAbilityDone = true;
    }
}
