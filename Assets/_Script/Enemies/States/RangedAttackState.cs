using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected S_EnemyRangedAttackState stateData;

    protected GameObject projectile;
    protected E2_Projectile projectileScript;
    public RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyRangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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

        projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        projectileScript = projectile.GetComponent<E2_Projectile>();
        stateData.projectileDetails.facingDirection = Movement.FacingDirection;
        projectileScript.FireProjectile(stateData.projectileDetails, stateData.projectileTravelDistance);
    }
}
