using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMultiShootState : EnemyState
{
    private ED_EnemyMultiShootState data;
    private int randomInt;
    private Transform attackPos;
    protected bool gotoNextState;

    public EnemyMultiShootState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, ED_EnemyMultiShootState data, Transform attackPos) : base(entity, stateMachine, animBoolName)
    {
        this.data = data;
        this.attackPos = attackPos;
    }

    public override void Enter()
    {
        base.Enter();

        randomInt = Random.Range(0, data.bullet.Length);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CollisionSenses.Ground)
        {
            Movement.SetVelocityZero();
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        GameObject obj = ObjectPoolManager.SpawnObject(data.bullet[randomInt].obj, attackPos.position, Quaternion.identity);
        if (obj.TryGetComponent(out IFireable scr))
        {
            Vector2 delta = ((Vector2)CheckPlayerSenses.IsPlayerInMaxAgroRange.transform.position) - (Vector2)attackPos.position;
            scr.Init(delta, data.bullet[randomInt].details.speed, data.bullet[randomInt].details);
            scr.Fire();
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        gotoNextState = true;
    }


}
