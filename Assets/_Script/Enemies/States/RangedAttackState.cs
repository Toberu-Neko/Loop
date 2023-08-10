using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected S_EnemyRangedAttackState stateData;

    protected GameObject projectile;
    private E2_Projectile projectileScript;
    public RangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyRangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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

        projectile = ObjectPoolManager.SpawnObject(stateData.projectile, attackPosition.position, attackPosition.rotation, ObjectPoolManager.PoolType.Projectiles);
        projectileScript = projectile.GetComponent<E2_Projectile>();
        projectileScript.FireProjectile(stateData.projectileDetails, stateData.projectileTravelDistance, Movement.FacingDirection);
    }
}
