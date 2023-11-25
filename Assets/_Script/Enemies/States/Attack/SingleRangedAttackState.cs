using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRangedAttackState : AttackState
{
    private ED_EnemyRangedAttackState stateData;

    public SingleRangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyRangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
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
            projectileScript.Init(delta.normalized, stateData.projectileDetails.speed, stateData.projectileDetails);
            projectileScript.Fire();
        }
        else
        {
            projectileScript.Init(Movement.ParentTransform.right, stateData.projectileDetails.speed, stateData.projectileDetails);
            projectileScript.Fire();
        }
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0;
    }
}
