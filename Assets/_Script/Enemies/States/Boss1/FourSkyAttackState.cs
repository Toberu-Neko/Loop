using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourSkyAttackState : EnemyState
{
    protected ED_FourSkyAttackState stateData;

    public bool IsAttackDone { get; private set; }

    private Transform playerPos;
    private Vector2[] attackPos;
    private IFireable[] fireables;

    private int attackCount = 0;
    private float spawnTime = 0f;
    private bool fireObjs;
    public FourSkyAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_FourSkyAttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        playerPos = CheckPlayerSenses.AllRnagePlayerRaycast.transform;
        attackCount= 0;
        spawnTime = 0f;
        fireObjs = false;

        fireables = new IFireable[4];

        attackPos = new Vector2[4];
        attackPos[0] = (Vector2)playerPos.position + Vector2.right * stateData.attackDistance;
        attackPos[1] = (Vector2)playerPos.position + Vector2.left * stateData.attackDistance;
        attackPos[2] = (Vector2)playerPos.position + Vector2.one * stateData.attackDistance;
        attackPos[3] = (Vector2)playerPos.position + new Vector2(-1f, 1f) * stateData.attackDistance;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Stats.Timer(spawnTime);

        if(Time.time >= spawnTime + stateData.spawnDelay && attackCount < attackPos.Length && !fireObjs)
        {
            SpawnObj(attackCount);
            attackCount++;
            spawnTime = Time.time;
        }

        if(fireObjs && Time.time >= spawnTime + stateData.fireDelay)
        {
            fireables[attackCount].Fire(attackPos[attackCount] - (Vector2)playerPos.position, stateData.details);

            if(attackCount == attackPos.Length - 1)
            {
                IsAttackDone = true;
            }
            attackCount++;
            spawnTime = Time.time;
        }
    }

    private void SpawnObj(int index)
    {
        GameObject obj = ObjectPoolManager.SpawnObject(stateData.projectileObjs[Random.Range(0, stateData.projectileObjs.Length)], attackPos[index], Quaternion.identity);
        fireables[index] = obj.GetComponent<IFireable>();

        if(index == attackPos.Length - 1)
        {
            fireObjs = true;
            attackCount = 0;
        }
    }

    public void ResetAttack() => IsAttackDone = false;
}
