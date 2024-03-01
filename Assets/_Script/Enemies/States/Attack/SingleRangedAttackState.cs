using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRangedAttackState : AttackState
{
    private ED_EnemyRangedAttackState stateData;
    private IFireable fireable;

    public SingleRangedAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_EnemyRangedAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        if(fireable == null)
        {
            GameObject projectile = ObjectPoolManager.SpawnObject(stateData.projectile, attackPosition.position, attackPosition.rotation, ObjectPoolManager.PoolType.Projectiles);
            fireable = projectile.GetComponent<IFireable>();

            if (CheckPlayerSenses.IsPlayerInMaxAgroRange && stateData.aimPlayer)
            {
                Vector2 delta = ((Vector2)CheckPlayerSenses.IsPlayerInMaxAgroRange.transform.position) - (Vector2)attackPosition.position;
                fireable.Init(stateData.projectileDetails.speed, stateData.projectileDetails);
                fireable.Fire(delta.normalized);
            }
            else
            {
                fireable.Init(stateData.projectileDetails.speed, stateData.projectileDetails);
                fireable.Fire(Movement.ParentTransform.right);
            }
        }
        else
        {
            if (CheckPlayerSenses.IsPlayerInMaxAgroRange && stateData.aimPlayer)
            {
                Vector2 delta = ((Vector2)CheckPlayerSenses.IsPlayerInMaxAgroRange.transform.position) - (Vector2)attackPosition.position;
                fireable.Init(stateData.projectileDetails.speed, stateData.projectileDetails);
                fireable.Fire(delta.normalized);
            }
            else
            {
                fireable.Init(stateData.projectileDetails.speed, stateData.projectileDetails);
                fireable.Fire(Movement.ParentTransform.right);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        fireable = null;
    }

    public void SetFireable(IFireable fireable)
    {
        this.fireable = fireable;
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown || EndTime == 0;
    }
}
