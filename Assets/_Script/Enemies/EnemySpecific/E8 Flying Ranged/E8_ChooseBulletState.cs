using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E8_ChooseBulletState : FlyingChooseSingleBulletState
{
    private Enemy8 enemy;
    private ED_ChooseSingleBulletState stateData;
    private Transform spawnPos;
    private IFireable fireable;

    private bool canSpawn;

    public E8_ChooseBulletState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_ChooseSingleBulletState stateData, Transform spawnPos, Enemy8 enemy) : base(entity, stateMachine, animBoolName, stateData, spawnPos)
    {
        this.enemy = enemy;
        this.stateData = stateData;
        this.spawnPos = spawnPos;
    }

    public override void Enter()
    {
        base.Enter();

        canSpawn = true;
    }


    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        if (canSpawn)
        {
            canSpawn = false;
            fireable = ObjectPoolManager.SpawnObject(stateData.bulletPrefab, spawnPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles).GetComponent<IFireable>();
        }
    }

    public override void Exit()
    {
        base.Exit();

        fireable = null;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        enemy.RangedAttackState.SetFireable(fireable);
        stateMachine.ChangeState(enemy.RangedAttackState);
    }

    public bool CheckCanEnterState()
    {
        return Time.time > StartTime + stateData.cooldown || StartTime == 0f;
    }
}
