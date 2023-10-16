using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceRoomAndExplodeState : EnemyFlyingStateBase
{
    private ED_SliceRoomAndExplodeState stateData;

    public bool IsAttackDone { get; private set; }
    private bool doRewind = false;

    private Vector2[,] explosivePositions;

    //ROW = LR COL = UD
    public SliceRoomAndExplodeState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_SliceRoomAndExplodeState stateData, BoxCollider2D bossRoom) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
        IsAttackDone = false;
        doRewind = false;

        explosivePositions = new Vector2[stateData.row, stateData.column];

        for(int i = 0; i < stateData.row; i++)
        {
            for (int j = 0; j < stateData.column; j++)
            {
                float xPerUnit = bossRoom.bounds.size.x / stateData.row;
                float yPerUnit = bossRoom.bounds.size.y / stateData.column;
                float x = bossRoom.bounds.min.x + bossRoom.bounds.size.x / stateData.row * i + xPerUnit / 2f;
                float y = bossRoom.bounds.min.y + bossRoom.bounds.size.y / stateData.column * j + yPerUnit / 2f;

                explosivePositions[i, j] = new Vector2(x, y);
            }
        }
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();

        for(int i = 0; i < stateData.row; i++)
        {
            for (int j = 0; j < stateData.column; j++)
            {
                GameObject obj = ObjectPoolManager.SpawnObject(stateData.bullets[0], explosivePositions[i, j], Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
                IFireable fireable = obj.GetComponent<IFireable>();
                fireable.Fire(Vector2.zero, stateData.details);
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();
    }

    public void ResetAttack() => IsAttackDone = false;
    public void SetDoRewindTrue() => doRewind = true;
}
