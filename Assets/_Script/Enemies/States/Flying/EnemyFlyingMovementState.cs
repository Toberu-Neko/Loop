using UnityEngine;

public class EnemyFlyingMovementState : EnemyFlyingStateBase
{
    protected ED_FlyingMovementState stateData;

    protected bool gotoIdleState;

    private Vector2 direction;
    public int RemainMoveCount { get; private set; } = 0;

    private bool getRandomDir;

    public EnemyFlyingMovementState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_FlyingMovementState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        getRandomDir = true;
        RemainMoveCount = Random.Range(stateData.minMoveCount, stateData.maxMoveCount);
    }

    public override void Enter()
    {
        base.Enter();

        RemainMoveCount--;

        gotoIdleState = false;

        if (getRandomDir)
        {
            direction = GetDirection().normalized;
        }

        Debug.Log(direction + " " + getRandomDir);

        getRandomDir = true;
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
        Vector2 t_direction = new(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f));
        int count = 0;

        while (CollisionSenses.IsDetectingWall(Movement.ParentTransform.position, t_direction, stateData.movementSpeed * stateData.moveTime))
        {
            if (count >= 15)
            {
                Debug.LogError("EnemyFlyingState: Can't find a direction to move");
                break;
            }

            count++;
            t_direction = new(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f));
        }

        Vector2 targetPos = (Vector2)Movement.ParentTransform.position + stateData.movementSpeed * stateData.moveTime * t_direction.normalized;

        t_direction = CheckTargetPos(t_direction, targetPos);

        return t_direction;
    }

    private Vector2 CheckTargetPos(Vector2 orgDir, Vector2 targetPos)
    {
        if (CollisionSenses.IsDetectingWall(targetPos, Vector2.down, entity.GetColliderY() / 2f))
        {
            Debug.Log("Original targetDir: " + orgDir);
            Vector2 newTargetPos = new(targetPos.x, targetPos.y + entity.GetColliderY() / 2f);
            orgDir = (newTargetPos - (Vector2)Movement.ParentTransform.position).normalized;
            Debug.Log("New targetDir: " + orgDir);

            return orgDir;
        }

        if (CollisionSenses.IsDetectingWall(targetPos, Vector2.up, entity.GetColliderY() / 2f))
        {
            Debug.Log("Original targetDir: " + orgDir);
            Vector2 newTargetPos = new(targetPos.x, targetPos.y - entity.GetColliderY() / 2f);
            orgDir = (newTargetPos - (Vector2)Movement.ParentTransform.position).normalized;
            Debug.Log("New targetDir: " + orgDir);

            return orgDir;
        }

        if (CollisionSenses.IsDetectingWall(targetPos, Vector2.left, entity.GetColliderX() / 2f))
        {
            Debug.Log("Original targetDir: " + orgDir);
            Vector2 newTargetPos = new(targetPos.x, targetPos.y + entity.GetColliderX() / 2f);
            orgDir = (newTargetPos - (Vector2)Movement.ParentTransform.position).normalized;
            Debug.Log("New targetDir: " + orgDir);

            return orgDir;
        }


        if (CollisionSenses.IsDetectingWall(targetPos, Vector2.right, entity.GetColliderX() / 2f))
        {
            Debug.Log("Original targetDir: " + orgDir);
            Vector2 newTargetPos = new(targetPos.x, targetPos.y - entity.GetColliderX() / 2f);
            orgDir = (newTargetPos - (Vector2)Movement.ParentTransform.position).normalized;
            Debug.Log("New targetDir: " + orgDir);

            return orgDir;
        }

        return orgDir;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
        getRandomDir = false;
    }

    public void ResetMoveCount() => RemainMoveCount = Random.Range(stateData.minMoveCount, stateData.maxMoveCount);
}
