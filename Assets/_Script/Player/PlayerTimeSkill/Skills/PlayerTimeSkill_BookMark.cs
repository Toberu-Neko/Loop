using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkill_BookMark : PlayerTimeSkillBase
{
    private List<PointInTime> pointsInTime;

    private bool isRecording;
    private GameObject mark;

    public PlayerTimeSkill_BookMark(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName) : base(player, manager, stateMachine, data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        pointsInTime = new();
        isRecording = false;
    }
    public override void Exit()
    {
        base.Exit();

        StopRewinding();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && 
            !isRecording && !Stats.IsRewindingPosition &&
            manager.CurrentEnergy > data.bookMarkCostPerSecond * Time.deltaTime)
        {
            player.InputHandler.UseTimeSkillInput();
            isRecording = true;

            mark = GameObject.Instantiate(data.bookMarkPrefab, player.transform.position, Quaternion.identity);
            SpriteRenderer ren = mark.GetComponent<SpriteRenderer>();
            ren.sprite = player.SR.sprite;
            if(Movement.FacingDirection == -1)
            {
               ren.flipX = true;
            }

        }
        else if (isRecording && (player.InputHandler.TimeSkillInput || manager.CurrentEnergy <= 0)) 
        {
            isRecording = false;
            StartRewinding();
        }

        if(isRecording)
        {
            manager.DecreaseEnergy(data.bookMarkCostPerSecond * Time.deltaTime);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isRecording)
        {
            Record();
        }
        else if (Stats.IsRewindingPosition)
        {
            RewindPosition();
        }
    }
    private float fixedDeltaTimer = 0;
    private void Record()
    {
        if (Time.fixedDeltaTime >= 0.02f || fixedDeltaTimer >= 0.02f)
        {
            if (pointsInTime.Count > Mathf.Round(data.rewindMaxTime / Time.fixedDeltaTime))
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

            for(int i = 1; i < data.bookmarkPlaySpeed; i++)
            {
                if(pointsInTime.Count > 1)
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
        if (mark)
        {
            GameObject.Destroy(mark);
        }

        Stats.SetInvincibleFalse();
    }
}
