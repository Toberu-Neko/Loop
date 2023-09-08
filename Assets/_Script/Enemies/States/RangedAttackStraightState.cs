using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackStraightState : AttackState
{
    protected S_EnemyRangedAttackState stateData;

    protected GameObject projectile;
    private EnemyProjectile projectileScript;

    public RangedAttackStraightState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyRangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        projectile = ObjectPoolManager.SpawnObject(stateData.projectile, attackPosition.position, attackPosition.rotation, ObjectPoolManager.PoolType.Projectiles);
        projectileScript = projectile.GetComponent<EnemyProjectile>();
        projectileScript.FireProjectile(stateData.projectileDetails, Movement.FacingDirection, Movement.ParentTransform.right);
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0;
    }
}
