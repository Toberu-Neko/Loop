using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipingState : AttackState
{
    S_EnemySnipingState stateData;
    private Transform player;
    protected bool goToIdleState;
    private Vector2 aimPointDelta;
    private Vector2 targetPos;

    private Vector2 v2WorkSpace;

    private bool firesShoot;
    private bool startShooting;
    private states state;
    private enum states 
    { 
        aiming,
        locked,
        reloading
    }


    private float lastShootTime;

    private DrawWire drawWire;

    public SnipingState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, S_EnemySnipingState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
        drawWire = core.GetCoreComponent<DrawWire>();
        lastShootTime = 0f;
    }

    public override void Enter()
    {
        base.Enter();

        startShooting = false;
        goToIdleState = false;
        player = null;
        firesShoot = true;
        state = states.reloading;
        entity.Anim.SetBool("isAiming", true);
    }
    public override void Exit()
    {
        base.Exit();

        drawWire.ClearPoints();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(state == states.reloading && Time.time >= lastShootTime + stateData.reloadTime)
        {
            state = states.aiming;
            StartTime = Time.time;
        }

        if (state == states.aiming)
        {
            if (CheckPlayerSenses.IsPlayerInMaxAgroRange && !player)
            {
                player = CheckPlayerSenses.IsPlayerInMaxAgroRange.collider.gameObject.transform;
                targetPos = player.position;
            }
            if (player)
            {
                float leftTime = stateData.aimTime - (Time.time - StartTime);
                v2WorkSpace.Set(0f, stateData.shakeCurve.Evaluate(leftTime / stateData.aimTime) * 2f);

                if (CheckPlayerSenses.CanSeePlayer)
                {
                    targetPos = Vector3.Slerp((Vector3)targetPos, player.position, (stateData.aimTime - leftTime) / stateData.aimTime);
                }
                aimPointDelta = (targetPos + v2WorkSpace - (Vector2)attackPosition.position).normalized;


                RaycastHit2D hit = Physics2D.Raycast(attackPosition.position, aimPointDelta, 30f, stateData.whatIsGround);
                if (hit)
                {
                    drawWire.SetPoints(attackPosition.position, hit.point);
                }
                else
                {
                    drawWire.SetPoints(attackPosition.position, attackPosition.position + ((Vector3)aimPointDelta * 100f)); 
                }
                drawWire.ChangeColor(stateData.aimColor);
                drawWire.RenderLine();
            }
        }

        if(state == states.aiming && Time.time >= StartTime + stateData.aimTime)
        {
            state = states.locked;
            StartTime = Time.time;
            // entity.Anim.SetBool("isAiming", false);

            Lock();
        }

        else if (state == states.locked && !startShooting && Time.time >= StartTime + stateData.freazeTime)
        {
            startShooting = true;
            entity.Anim.SetTrigger("shoot");
        }

        else if(state == states.reloading && Time.time >= lastShootTime + stateData.reloadTime)
        {
            player = null;
            // entity.Anim.SetBool("isAiming", true);
            StartTime = Time.time;
        }

        if (!CheckPlayerSenses.CanSeePlayer && state == states.reloading && !firesShoot)
        {
            goToIdleState = true;
        }

        Timer(lastShootTime);
    }

    private void Lock()
    {
        drawWire.ChangeColor(stateData.lockColor);
    }

    private void Shoot()
    {
        state = states.reloading;
        firesShoot = false;
        startShooting = false;
        lastShootTime = Time.time;
        drawWire.ClearPoints();
        drawWire.RenderLine();

        GameObject bulletObj = ObjectPoolManager.SpawnObject(stateData.bulletPrefab, attackPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
        EnemyProjectile script = bulletObj.GetComponent<EnemyProjectile>();
        script.FireProjectile(stateData.bulletDetails, Movement.FacingDirection, aimPointDelta, stateData.bulletDetails.animator);
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
        Shoot();
    }

    public bool CheckCanAttack()
    {
        return Time.time >= lastShootTime + stateData.reloadTime || EndTime == 0f;
    }
}
