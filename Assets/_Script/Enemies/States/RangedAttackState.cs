using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    private S_EnemyRangedAttackState stateData;

    public RangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemyRangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        GameObject projectile = ObjectPoolManager.SpawnObject(stateData.projectile, attackPosition.position, attackPosition.rotation, ObjectPoolManager.PoolType.Projectiles);
        EnemyProjectile projectileScript = projectile.GetComponent<EnemyProjectile>();
        if (CheckPlayerSenses.IsPlayerInMaxAgroRange && stateData.aimPlayer)
        {
            Vector2 delta = ((Vector2)CheckPlayerSenses.IsPlayerInMaxAgroRange.collider.bounds.center) - (Vector2)Movement.ParentTransform.position;
            projectileScript.FireProjectile(stateData.projectileDetails, Movement.FacingDirection, delta.normalized);
        }
        else
        {
            projectileScript.FireProjectile(stateData.projectileDetails, Movement.FacingDirection, Movement.ParentTransform.right);
        }
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0;
    }
}
