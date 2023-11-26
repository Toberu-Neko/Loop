using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B1_ChooseRandomBulletState : ChooseRandomBulletState
{
    private readonly Boss1 boss;
    private Transform spawnPos;
    private GameObject spawnedObj;
    private IFireable fireable;

    public B1_ChooseRandomBulletState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_ChooseRandomBulletState stateData, Transform spawnPos, Boss1 boss) : base(entity, stateMachine, animBoolName, stateData, spawnPos)
    {
        this.boss = boss;
        this.spawnPos = spawnPos;
    }

    public override void Exit()
    {
        base.Exit();

        if (!isAnimationFinished && spawnedObj != null && spawnedObj.activeInHierarchy)
        {
            ObjectPoolManager.ReturnObjectToPool(spawnedObj);
        }

        spawnedObj = null;
        fireable = null;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        switch (bulletIndex)
        {
            case 0:
                boss.BlueRangedAttackState.SetFireable(fireable);
                stateMachine.ChangeState(boss.BlueRangedAttackState);
                break;
            case 1:
                boss.RedRangedAttackState.SetFireable(fireable);
                stateMachine.ChangeState(boss.RedRangedAttackState);
                break;
            case 2:
                boss.transform.position = boss.SkyTeleportPos.position;
                ObjectPoolManager.ReturnObjectToPool(spawnedObj);
                stateMachine.ChangeState(boss.FlyingIdleState);
                break;
            default:
                Debug.LogError("Bullet index out of range");
                stateMachine.ChangeState(boss.PlayerDetectedMoveState);
                break;
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        switch(bulletIndex)
        {
            case 0:
            default:
                spawnedObj = ObjectPoolManager.SpawnObject(stateData.blueAttackPrefab, spawnPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
                fireable = spawnedObj.GetComponent<IFireable>();
                break;
            case 1:
                spawnedObj = ObjectPoolManager.SpawnObject(stateData.redAttackPrefab, spawnPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
                fireable = spawnedObj.GetComponent<IFireable>();
                break;
            case 2:
                spawnedObj = ObjectPoolManager.SpawnObject(stateData.greenAttackPrefab, spawnPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
                break;
        }
    }
}

