using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnblockableAttackState : AttackState
{
    protected ED_EnemyMeleeAttackState stateData;
    public UnblockableAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyMeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        DoDamageToDamageList(stateData.attackDamage, stateData.staminaAttackDamage, stateData.knockbackAngle, stateData.knockbackStrength, false);

    }
}
