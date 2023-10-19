using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlayerAttackState : EnemyState
{
    private ED_AbovePlayerAttackState stateData;
    public bool IsAttackDone { get; private set; }

    private Transform playerPos;
    private Vector2[] attackPos;
    private EP_Rewind[] projectiles;

    private bool canSpawn = true;
    private float fireTime;
    private int spawnCount;
    private bool doRewind = false;
    private State state;

    private enum State
    {
        attack,
        rewind,
        end
    }
    public AbovePlayerAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_AbovePlayerAttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();

        playerPos = CheckPlayerSenses.AllRnagePlayerRaycast.transform;
        if (playerPos == null)
        {
            Debug.LogError("FourSkyAttackState: PlayerPos is null");
            IsAttackDone = true;
            return;
        }

        state = State.attack;

        canSpawn = true;
        doRewind = false;
        spawnCount = 0;
        fireTime = 0f;

        attackPos = new Vector2[stateData.spawnCount];
        projectiles = new EP_Rewind[stateData.spawnCount];

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();

        if (state == State.attack)
        {
            if (spawnCount < stateData.spawnCount)
            {
                if (Time.time >= fireTime + stateData.spawnDelay && canSpawn)
                {
                    canSpawn = false;
                    fireTime = Time.time;
                    attackPos[spawnCount] = (Vector2)playerPos.position + Vector2.up * stateData.attackDistance;
                    SpawnObj(spawnCount, attackPos[spawnCount]);
                }

                if (Time.time >= fireTime + stateData.fireDelay && !canSpawn)
                {
                    canSpawn = true;
                    fireTime = Time.time;
                    projectiles[spawnCount].Fire(Vector2.down, stateData.details.speed, stateData.details);
                    spawnCount++;
                }
            }
            else
            {
                state = State.rewind;
                fireTime = Time.time;
            }
        }

        if(state == State.rewind)
        {
            if (Time.time >= fireTime + stateData.rewindDelay)
            {
                foreach (EP_Rewind obj in projectiles)
                {
                    obj.Rewind(doRewind);
                }
            }

            if (doRewind)
            {
                state = State.end;
            }
            else
            {
                IsAttackDone = true;
            }
        }

        if(state == State.end)
        {
            if(Time.time >= fireTime + stateData.rewindDelay)
            {
                IsAttackDone = true;
            }
        }

    }
    public void ResetAttack() => IsAttackDone = false;
    public void SetDoRewindTrue() => doRewind = true;

    private void SpawnObj(int index, Vector2 attackPos)
    {
        GameObject obj = ObjectPoolManager.SpawnObject(stateData.projectileObjs[Random.Range(0, stateData.projectileObjs.Length)], attackPos, Quaternion.identity);

        projectiles[index] = obj.GetComponent<EP_Rewind>();
        projectiles[index].SetDetails(stateData.details, doRewind);
    }
}
