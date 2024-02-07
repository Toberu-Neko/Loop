using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkill_TimeStopThrow : PlayerTimeSkillBase
{
    private bool equipped;
    private bool throwable;
    private bool charging;
    private float throwVelocity;
    private TimeStopProjectile script;
    public PlayerTimeSkill_TimeStopThrow(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName) : base(player, manager, stateMachine, data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        SkillName = Data.timeStopThrowSkillName;
        equipped = false;
        throwable = true;
        charging = false;
        throwVelocity = Data.minThrowVelocity;
    }

    public override void Exit()
    {
        base.Exit();

        UnEquip();
        HandleObjFlyBack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && !equipped && throwable && manager.CurrentEnergy >= Data.timeStopThrowCost)
        {
            player.InputHandler.UseTimeSkillInput();
            equipped = true;
            Stats.SetAttackable(false);
        }
        else if(player.InputHandler.TimeSkillInput && equipped)
        {
            UnEquip();
        }

        if (equipped && throwable && player.InputHandler.AttackInput)
        {
            player.InputHandler.UseAttackInput();
            throwable = false;
            charging = true;
        }

        if(charging)
        {
            if(throwVelocity < Data.maxThrowVelocity)
            {
                throwVelocity += Data.throwVelocityIncreaseRate * Time.deltaTime;
            }

            for (int i = 0; i < Data.numberOfPredictLineObj; i++)
            {
                manager.PredictLineTransforms[i].gameObject.SetActive(true);
                manager.PredictLineTransforms[i].position = PredictObjPosition(Data.spaceBetweenPredictLineObj * i);
            }

            if (!player.InputHandler.HoldAttackInput)
            {
                manager.DecreaseEnergy(Data.timeStopThrowCost);
                charging = false;
                UnEquip();

                for (int i = 0; i < Data.numberOfPredictLineObj; i++)
                {
                    manager.PredictLineTransforms[i].gameObject.SetActive(false);
                }

                GameObject obj = ObjectPoolManager.SpawnObject(Data.timeStopThrowObj, player.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
                script = obj.GetComponent<TimeStopProjectile>();
                script.Fire(throwVelocity, player.InputHandler.RawMouseDirectionInput, Data.throwStopTime, Data.gravityScale, manager.transform);
                script.OnReturnToPlayer += HandleObjFlyBack;

                throwVelocity = Data.minThrowVelocity;
            }
        }
    }

    private void HandleObjFlyBack()
    {
        throwable = true;
        if (script)
        {
            script.OnReturnToPlayer -= HandleObjFlyBack;        
        }
    }

    private void UnEquip()
    {
        player.InputHandler.UseTimeSkillInput();
        equipped = false;
        // Debug.Log("Unequipped");
        Stats.SetAttackable(true);
    }

    private Vector2 PredictObjPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + 
            (t * throwVelocity * player.InputHandler.RawMouseDirectionInput) + 
            (t * t) * 0.5f * (Physics2D.gravity * Data.gravityScale);
        return position;
    }
}
