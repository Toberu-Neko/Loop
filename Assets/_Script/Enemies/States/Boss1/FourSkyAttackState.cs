using UnityEngine;

public class FourSkyAttackState : EnemySkyAttackBase
{
    protected ED_FourSkyAttackState stateData;

    private Transform playerPos;
    private Vector2[] attackPos;
    private EP_Rewind[] projectiles;

    private int attackCount = 0;
    private float spawnTime = 0f;
    private float allGroundedTime = 0f;
    private bool firstTimeAllGrounded;
    private bool fireObjs;

    private bool startRewind;
    public FourSkyAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_FourSkyAttackState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        doRewind = false;
    }

    public override void Enter()
    {
        base.Enter();

        playerPos = CheckPlayerSenses.AllRnagePlayerRaycast.transform;

        if(playerPos == null)
        {
            Debug.LogError("FourSkyAttackState: PlayerPos is null");
            isAttackDone = true;
            return;
        }
        attackCount= 0;
        spawnTime = 0f;
        allGroundedTime = 0f;
        fireObjs = false;
        startRewind = false;
        firstTimeAllGrounded = false;

        projectiles = new EP_Rewind[4];

        attackPos = new Vector2[4];
        attackPos[0] = (Vector2)playerPos.position + Vector2.right * stateData.attackDistance;
        attackPos[1] = (Vector2)playerPos.position + Vector2.left * stateData.attackDistance;
        attackPos[2] = (Vector2)playerPos.position + Vector2.one.normalized * stateData.attackDistance;
        attackPos[3] = (Vector2)playerPos.position + new Vector2(-1f, 1f).normalized * stateData.attackDistance;
    }

    public override void Exit()
    {
        base.Exit();

        foreach (var item in projectiles)
        {
            if (item != null)
                item.Rewind(false);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Stats.Timer(spawnTime);
        Movement.SetVelocityZero();

        if(Time.time >= spawnTime + stateData.spawnDelay && attackCount < attackPos.Length && !fireObjs)
        {
            SpawnObj(attackCount);
            attackCount++;
            spawnTime = Time.time;

            if(attackCount == attackPos.Length)
            {
                attackCount = 0;
                fireObjs = true;
            }
        }

        if(fireObjs && Time.time >= spawnTime + stateData.fireDelay && !startRewind)
        {
            Vector2 dir = (Vector2)playerPos.position - attackPos[attackCount];
            projectiles[attackCount].Fire(dir.normalized, stateData.details.speed, stateData.details);

            attackCount++;
            spawnTime = Time.time;

            if (attackCount == attackPos.Length)
            {
                attackCount = 0;

                startRewind = true;
            }
        }


        if (startRewind)
        {
            bool allGrounded = true;

            foreach (var item in projectiles)
            {
                if (!item.HasHitGround)
                {
                    allGrounded = false;
                }
            }
            if (allGrounded && !firstTimeAllGrounded)
            {
                Debug.Log("All Grounded");
                firstTimeAllGrounded = true;
                allGroundedTime = Time.time;
            }

            if (Time.time >= allGroundedTime + stateData.rewindDelay && firstTimeAllGrounded)
            {
                if (!doRewind)
                {
                    foreach (var item in projectiles)
                    {
                        item.Rewind(false);
                    }

                    isAttackDone = true;
                }

                else
                {
                    projectiles[attackCount].Rewind();
                    allGroundedTime = Time.time;
                    attackCount++;

                    if (attackCount == attackPos.Length)
                    {
                        isAttackDone = true;
                    }
                }

            }
        }
    }

    private void SpawnObj(int index)
    {
        GameObject obj = ObjectPoolManager.SpawnObject(stateData.projectileObjs[Random.Range(0, stateData.projectileObjs.Length)], attackPos[index], Quaternion.identity);

        projectiles[index] = obj.GetComponent<EP_Rewind>();
        projectiles[index].SetDetails(stateData.details, doRewind);
    }
}
