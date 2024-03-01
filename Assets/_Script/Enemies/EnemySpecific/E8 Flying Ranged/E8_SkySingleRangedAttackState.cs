using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_SkySingleRangedAttackState : FlyingSingleRangedAttackState
{
    private Enemy8 enemy;
    public E8_SkySingleRangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyRangedAttackState stateData, Enemy8 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        enemy.MoveState.SetDirection(new Vector2(-Movement.FacingDirection, 0f));
        stateMachine.ChangeState(enemy.MoveState);
    }
}
