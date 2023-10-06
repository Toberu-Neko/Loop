using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAndMultiAttackState : EnemyState
{
    protected ED_EnemyJumpAndMultiAttackState stateData;
    private Transform attackPos;

    public JumpAndMultiAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyJumpAndMultiAttackState stateData, Transform attackPos) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.attackPos = attackPos;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocity(stateData.jumpForce, stateData.jumpAngle, -Movement.FacingDirection);
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityZero();
        Movement.SetRBKinematic();
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        Movement.SetRBDynamic();
        Movement.SetVelocity(15f, Vector2.one, -Movement.FacingDirection);

        //TODO: Shoot four bullets
        for (int i = 0; i < stateData.attackAmount; i++)
        {
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        int index = Random.Range(0, stateData.bullets.Length);
        GameObject projectile = ObjectPoolManager.SpawnObject(stateData.bullets[index].obj, attackPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
        IFireable projectileScript = projectile.GetComponent<IFireable>();

        Vector2 shootAngle = new(Random.Range(0.1f, 0.94f) * Movement.FacingDirection, Random.Range(-0.3f, -0.15f));

        projectileScript.Fire(shootAngle.normalized, stateData.bullets[index].details);
    }

    public bool CanChangeState()
    {
        return Time.time - EndTime >= stateData.attackCooldown || StartTime == 0;
    }
}
