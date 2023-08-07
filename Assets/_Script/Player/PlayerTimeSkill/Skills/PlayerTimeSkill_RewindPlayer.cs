using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerTimeSkill_RewindPlayer : PlayerTimeSkillBase
{
    private List<PointInTime> pointsInTime;
    public PlayerTimeSkill_RewindPlayer(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName) : base(player, manager, stateMachine, data, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        pointsInTime = new();
    }

    public override void Exit()
    {
        base.Exit();

        StopRewinding();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && !Stats.IsRewindingPosition && manager.CurrentEnergy > data.rewindCostPerSecond * Time.deltaTime)
        {
            player.InputHandler.UseTimeSkillInput();
            StartRewinding();
        }
        if (Stats.IsRewindingPosition)
        {
            if(!player.InputHandler.TimeSkillHoldInput || manager.CurrentEnergy <= 0)
                StopRewinding();
            else
                manager.DecreaseEnergy(data.rewindCostPerSecond * Time.deltaTime);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (Stats.IsRewindingPosition)
        {
            RewindPosition();
        }
        else
        {
            Record();
        }
    }


    private float fixedDeltaTimer = 0;
    private void Record()
    {
        if (Time.fixedDeltaTime >= 0.02f || fixedDeltaTimer >= 0.02f)
        {
            if(pointsInTime.Count > Mathf.Round(data.rewindMaxTime / Time.fixedDeltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }
            pointsInTime.Insert(0, new PointInTime((Vector2)player.transform.position, player.transform.rotation, Movement.FacingDirection, player.SR.sprite));
            fixedDeltaTimer = 0f;
        }
        else
        {
            fixedDeltaTimer += Time.fixedDeltaTime;
        }
    }

    private void RewindPosition()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime point = pointsInTime[0];
            Movement.SetPosition(point.position, point.rotation, point.facingDirection);
            player.SR.sprite = point.sprite;

            for (int i = 1; i < data.rewindPlaySpeed; i++)
            {
                if (pointsInTime.Count > 1)
                    pointsInTime.RemoveAt(0);
            }
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewinding();
            return;
        }
    }
    private void StartRewinding()
    {
        Stats.SetRewindingPosition(true);
        player.RB.isKinematic = true;
        player.Anim.enabled = false;

        Stats.SetInvincibleTrue();
    }
    private void StopRewinding()
    {
        Stats.SetRewindingPosition(false);
        player.RB.isKinematic = false;
        player.Anim.enabled = true;

        Stats.SetInvincibleFalse();
    }
}


[Serializable]
public class PointInTime
{
    public Vector2 position;
    public Quaternion rotation;
    public int facingDirection;
    public Sprite sprite;

    public PointInTime(Vector2 position, Quaternion rotation, int facingDirection, Sprite sprite)
    {
        this.position = position;
        this.rotation = rotation;
        this.facingDirection = facingDirection;
        this.sprite = sprite;
    }
}
