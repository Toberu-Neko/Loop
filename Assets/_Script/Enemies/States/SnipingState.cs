using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipingState : AttackState
{
    S_EnemySnipingState stateData;
    private Transform player;
    protected bool goToIdleState;
    private bool isAiming;
    private bool isLocked;
    private bool isReloading;
    private Vector2 aimPointDelta;
    private Vector2 targetPos;
    private Vector3 v3WorkSpace;
    private Vector2 v2WorkSpace;

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

        isAiming = true;
        isLocked = false;
        isReloading = false;
        goToIdleState = false;
        player = null;
        v3WorkSpace = Vector3.zero;
        entity.Anim.SetBool("isAiming", true);
    }
    public override void Exit()
    {
        base.Exit();

        drawWire.ClearPoints();
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAiming && Time.time >= lastShootTime + stateData.reloadTime)
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

                targetPos = Vector3.Slerp((Vector3)targetPos, player.position, (stateData.aimTime - leftTime) / stateData.aimTime);
                aimPointDelta = ((Vector2)targetPos + v2WorkSpace - (Vector2)attackPosition.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(attackPosition.position, aimPointDelta, 30f, stateData.whatIsGround);
                if (hit)
                {
                    drawWire.SetPoints(attackPosition.position, hit.point + (Vector2)v3WorkSpace);
                }
                else
                {
                    drawWire.SetPoints(attackPosition.position, attackPosition.position + ((Vector3)aimPointDelta * 100f)); 
                }
                drawWire.ChangeColor(stateData.aimColor);
                drawWire.RenderLine();
            }
        }

        if(isAiming && Time.time >= StartTime + stateData.aimTime)
        {
            isAiming = false;
            isLocked = true;
            StartTime = Time.time;
            // entity.Anim.SetBool("isAiming", false);

            Lock();
        }

        else if (!isAiming && isLocked && Time.time >= StartTime + stateData.freazeTime)
        {
            isLocked = false;
            entity.Anim.SetTrigger("shoot");
        }

        else if(isReloading && !isAiming && !isLocked && Time.time >= lastShootTime + stateData.reloadTime)
        {
            isReloading = false;
            player = null;
            isAiming = true;
            // entity.Anim.SetBool("isAiming", true);
            StartTime = Time.time;
        }

        if (!isPlayerInMaxAgroRange && isReloading)
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
        isReloading = true;
        lastShootTime = Time.time;
        drawWire.ClearPoints();
        drawWire.RenderLine();

        GameObject bulletObj = ObjectPoolManager.SpawnObject(stateData.bulletPrefab, attackPosition.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
        EnemyProjectile script = bulletObj.GetComponent<EnemyProjectile>();
        script.FireProjectile(stateData.bulletDetails, Movement.FacingDirection, aimPointDelta);
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
        Shoot();
    }
}
