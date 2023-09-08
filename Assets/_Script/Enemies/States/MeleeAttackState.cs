using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected S_EnemyMeleeAttackState stateData;

    public MeleeAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyMeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        Collider2D detectedObjects = Physics2D.OverlapCircle(attackPosition.position, stateData.attackRadius, stateData.whatIsPlayer);
        if (detectedObjects)
        {
            if (detectedObjects.TryGetComponent<IDamageable>(out var dam))
            {
                dam.Damage(stateData.attackDamage, entity.GetPosition());
            }

            if (detectedObjects.TryGetComponent<IKnockbackable>(out var knockbackable))
            {
                knockbackable.Knockback(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, (Vector2)core.transform.position);
            }

            if (detectedObjects.TryGetComponent<IStaminaDamageable>(out var staminaDamageable))
            {
                staminaDamageable.TakeStaminaDamage(stateData.staminaAttackDamage, entity.GetPosition());
            }
        }

    }
}
