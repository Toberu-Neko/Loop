using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovementState : EnemyFlyingStateBase
{
    protected ED_FlyingMovementState stateData;

    protected bool movementFinished;
    protected bool gotoIdleState;

    private Vector2 direction;
    public int RemainMoveCount { get; private set; } = 0;

    public EnemyFlyingMovementState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_FlyingMovementState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        RemainMoveCount = Random.Range(stateData.minMoveCount, stateData.maxMoveCount);
    }

    public override void Enter()
    {
        base.Enter();

        RemainMoveCount--;

        movementFinished = false;
        gotoIdleState = false;

        direction = GetDirection().normalized;
    }


    public override void LogicUpdate()
    {
        base.LogicUpdate();

        int dir = direction.x > 0 ? 1 : -1;

        Movement.CheckIfShouldFlip(dir);
        Movement.SetVelocity(stateData.movementSpeed * direction);


        if (Time.time >= StartTime + stateData.moveTime)
        {
            gotoIdleState = true;
        }
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = new(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f));
        int count = 0;

        while (CollisionSenses.IsDetectingWall(direction, stateData.movementSpeed * stateData.moveTime))
        {
            if (count >= 15)
            {
                Debug.LogError("EnemyFlyingState: Can't find a direction to move");
                break;
            }

            count++;
            direction = new(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f));
        }

        return direction;
    }

    public void ResetMoveCount() => RemainMoveCount = Random.Range(stateData.minMoveCount, stateData.maxMoveCount);
}
