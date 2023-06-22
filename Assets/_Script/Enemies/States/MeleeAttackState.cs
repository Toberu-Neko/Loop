using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected S_EnemyMeleeAttackState stateData;

    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyMeleeAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, stateData.meleeAttackRadius, stateData.whatIsPlayer);
        foreach (Collider2D collider in detectedObjects)
        {
            if (collider.TryGetComponent<IDamageable>(out var dam))
            {
                dam.Damage(stateData.meleeAttackDamage, entity.GetPosition());
            }

            if (collider.TryGetComponent<IKnockbackable>(out var knockbackable))
            {
                knockbackable.Knockback(stateData.knockbackAngle, stateData.knockbackStrength, Movement.FacingDirection, (Vector2)core.transform.position);
            }
        }
    }
}
