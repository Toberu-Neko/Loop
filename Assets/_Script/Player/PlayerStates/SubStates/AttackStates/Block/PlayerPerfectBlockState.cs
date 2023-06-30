using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPerfectBlockState : PlayerAttackState
{
    public PlayerPerfectBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

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

            IKnockbackable knockbackable = enemyCollider.GetComponentInChildren<IKnockbackable>();
            if (knockbackable != null)
            {
                knockbackable.Knockback(playerData.perfectBlockKnockbackAngle ,playerData.perfectBlockKnockbackForce, direction, (Vector2)core.transform.position);
            }
            
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
}
