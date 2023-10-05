using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    private ED_EnemyRangedAttackState stateData;

    public RangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyRangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        GameObject projectile = ObjectPoolManager.SpawnObject(stateData.projectile, attackPosition.position, attackPosition.rotation, ObjectPoolManager.PoolType.Projectiles);
        IFireable projectileScript = projectile.GetComponent<IFireable>();
        if (CheckPlayerSenses.IsPlayerInMaxAgroRange && stateData.aimPlayer)
        {
            Vector2 delta = ((Vector2)CheckPlayerSenses.IsPlayerInMaxAgroRange.transform.position) - (Vector2)attackPosition.position;
            projectileScript.Fire(delta.normalized, stateData.projectileDetails);
        }
        else
        {
            projectileScript.Fire(Movement.ParentTransform.right, stateData.projectileDetails);
        }
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0;
    }
}
