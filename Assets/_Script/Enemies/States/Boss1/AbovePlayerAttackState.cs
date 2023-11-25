using UnityEngine;

public class AbovePlayerAttackState : EnemySkyAttackBase
{
    private ED_AbovePlayerAttackState stateData;

    private Transform playerPos;
    private Vector2[] attackPos;
    private EP_Rewind[] projectiles;

    private bool canSpawn = true;
    private float fireTime;
    private int spawnCount;
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
        doRewind = true;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();

        playerPos = CheckPlayerSenses.AllRnagePlayerRaycast.transform;
        if (playerPos == null)
        {
            Debug.LogError("FourSkyAttackState: PlayerPos is null");
            isAttackDone = true;
            return;
        }

        state = State.attack;

        canSpawn = true;
        spawnCount = 0;
        fireTime = 0f;

        attackPos = new Vector2[stateData.spawnCount];
        projectiles = new EP_Rewind[stateData.spawnCount];

    }

    public override void Exit()
    {
        base.Exit();


        foreach (var item in projectiles)
        {
            if(item != null)
                item.Rewind(false);
        }
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
                    projectiles[spawnCount].Init(stateData.details.speed, stateData.details);
                    projectiles[spawnCount].Fire(Vector2.down);
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
                    if (!obj.HasHitGround)
                    {
                        return;
                    }
                }

                foreach (EP_Rewind obj in projectiles)
                {
                    obj.Rewind(doRewind);
                }

                if (doRewind)
                {
                    state = State.end;
                }
                else
                {
                    isAttackDone = true;
                }
            }
        }

        if(state == State.end)
        {
            foreach (EP_Rewind obj in projectiles)
            {
                if (obj.gameObject.activeInHierarchy)
                {
                    return;
                }
            }

            isAttackDone = true;
        }

    }

    private void SpawnObj(int index, Vector2 attackPos)
    {
        GameObject obj = ObjectPoolManager.SpawnObject(stateData.projectileObjs[Random.Range(0, stateData.projectileObjs.Length)], attackPos, Quaternion.identity);

        projectiles[index] = obj.GetComponent<EP_Rewind>();
        projectiles[index].SetDetails(stateData.details, doRewind);
    }
}
