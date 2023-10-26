using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAttackState : AttackState
{
    protected ED_MultiAttackState stateData;

    private int count = 0;
    public MultiAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_MultiAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        count = 0;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        WeaponAttackDetails details = stateData.details[count];

        DoDamageToDamageList(details.damageAmount, details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce, false);

        count++;
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0f;
    }


}
