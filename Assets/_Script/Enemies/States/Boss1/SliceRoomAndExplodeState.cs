using System.Collections.Generic;
using UnityEngine;

public class SliceRoomAndExplodeState : EnemyFlyingStateBase
{
    private ED_SliceRoomAndExplodeState stateData;

    public bool IsAttackDone { get; private set; }
    private bool doRewind = false;

    private Transform attackPos;
    private List<Vector2> orgExplosivePositions;
    private List<Vector2> explosivePositions;
    private int objPerSpawn;
    private int objCount = 0;
    private int currentSpawnCount;
    
    private float spawnTime = 0f;
    private List<IStaticProjectile> projectiles;

    private State state;
    private enum State
    {
        Spawn,
        Wait
    }

    //ROW = LR COL = UD
    public SliceRoomAndExplodeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_SliceRoomAndExplodeState stateData, BoxCollider2D bossRoom, Transform attackPos) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        this.attackPos = attackPos;
        IsAttackDone = false;
        doRewind = false;
        objPerSpawn = stateData.row * stateData.column / stateData.spawnCount;
        orgExplosivePositions = new();

        for (int i = 0; i < stateData.row; i++)
        {
            for (int j = 0; j < stateData.column; j++)
            {
                float xPerUnit = bossRoom.bounds.size.x / stateData.row;
                float yPerUnit = bossRoom.bounds.size.y / stateData.column;
                float x = bossRoom.bounds.min.x + bossRoom.bounds.size.x / stateData.row * i + xPerUnit / 2f;
                float y = bossRoom.bounds.min.y + bossRoom.bounds.size.y / stateData.column * j + yPerUnit / 2f;

                orgExplosivePositions.Add(new Vector2(x, y));
            }
        }
    }

    public override void Enter()
    {
        base.Enter();

        projectiles = new();

        explosivePositions = orgExplosivePositions;

        Movement.SetVelocityZero();
        state = State.Spawn;
        objCount = 0;
        currentSpawnCount = 1;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();

        switch (state)
        {
            case State.Spawn:
                spawnTime = Stats.Timer(spawnTime);

                if (Time.time >= spawnTime + stateData.spawnDelay && (stateData.row * stateData.column - objCount >= objPerSpawn))
                {
                    if (!doRewind)
                    {
                        Spawn(stateData.explodeDelay);
                    }
                    else
                    {
                        float firstDelay = stateData.explodeDelay - stateData.spawnDelay * (stateData.spawnCount - 1);

                        Spawn(firstDelay + stateData.spawnDelay * (stateData.spawnCount - currentSpawnCount) * 2f);
                    }
                }
                else if(stateData.row * stateData.column - objCount < objPerSpawn)
                {
                    state = State.Wait;
                }
                break;

            case State.Wait:
                List<IStaticProjectile> temp = new();

                foreach (var item in projectiles)
                {
                    if (item.Exploded())
                    {
                        temp.Add(item);
                    }
                }

                foreach (var item in temp)
                {
                    projectiles.Remove(item);
                }

                if(projectiles.Count == 0)
                {
                    IsAttackDone = true;
                }
                break;
        }

    }

    private void Spawn(float delay)
    {
        spawnTime = Time.time;
        currentSpawnCount++;
        for (int i = 0; i < objPerSpawn; i++)
        {
            objCount++;
            Vector2 targetPos = explosivePositions[Random.Range(0, explosivePositions.Count)];
            SpawnSingleObj(targetPos, delay);
            explosivePositions.Remove(targetPos);
        }
    }

    private void SpawnSingleObj(Vector2 targetPosition, float delay)
    {
        int index = Random.Range(0, stateData.bullets.Length);
        GameObject obj = ObjectPoolManager.SpawnObject(stateData.bullets[index].obj, attackPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
        IFireable fireable = obj.GetComponent<IFireable>();
        IStaticProjectile staticScript = obj.GetComponent<IStaticProjectile>();

        projectiles.Add(staticScript);

        Vector2 direction = targetPosition - (Vector2)attackPos.position;

        float distance = Vector2.Distance(targetPosition, (Vector2)attackPos.position);

        float speed = distance / stateData.flyTime;

        fireable.Fire(direction.normalized, speed, stateData.bullets[index].details);
        staticScript.Init(targetPosition, delay);
    }

    public void ResetAttack() => IsAttackDone = false;
    public void SetDoRewindTrue() => doRewind = true;
}
