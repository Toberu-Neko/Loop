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
    

    private float spawnTime = 0f;

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
        objPerSpawn = stateData.row * stateData.column / 3;
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

        explosivePositions = orgExplosivePositions;
        Movement.SetVelocityZero();
        state = State.Spawn;
        objCount = 0;
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
                    Spawn();
                }
                else if(stateData.row * stateData.column - objCount < objPerSpawn)
                {
                    state = State.Wait;
                }
                break;

            case State.Wait:
                break;
        }

    }

    private void Spawn()
    {
        spawnTime = Time.time;
        for (int i = 0; i < objPerSpawn; i++)
        {
            objCount++;
            Vector2 targetPos = explosivePositions[Random.Range(0, explosivePositions.Count)];
            SpawnSingleObj(targetPos);
            explosivePositions.Remove(targetPos);
        }
    }

    private void SpawnSingleObj(Vector2 targetPosition)
    {
        GameObject obj = ObjectPoolManager.SpawnObject(stateData.bullets[0], attackPos.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
        EP_BlueStatic fireable = obj.GetComponent<EP_BlueStatic>();

        Vector2 direction = targetPosition - (Vector2)attackPos.position;
        fireable.Fire(direction.normalized, stateData.details);
        fireable.Init(targetPosition, 3f);
    }

    public void ResetAttack() => IsAttackDone = false;
    public void SetDoRewindTrue() => doRewind = true;
}
