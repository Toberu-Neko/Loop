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
    
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        projectile = GameObject.Instantiate(stateData.projectile, attackPosition.position, attackPosition.rotation);
        projectileScript = projectile.GetComponent<E2_Projectile>();
        projectileScript.FireProjectile(stateData.projectileDetails, stateData.projectileTravelDistance, Movement.FacingDirection);
    }
}
