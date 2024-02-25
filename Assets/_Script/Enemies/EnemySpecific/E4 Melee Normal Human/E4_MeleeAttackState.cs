using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E4_MeleeAttackState : SingleMeleeAttackState
{
    private Enemy4 enemy;

    public E4_MeleeAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyMeleeAttackState stateData, Enemy4 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        AudioManager.instance.PlaySoundFX(enemy.stateData.attackSFX, Movement.ParentTransform, AudioManager.SoundType.threeD);
    }
}
