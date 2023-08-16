using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlayerTimeSkill_BulletTimeRanged : PlayerTimeSkillBase
{
    private bool started;
    private float startTime;
    private List<ITimeSlowable> timeSlowables;

    public PlayerTimeSkill_BulletTimeRanged(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName) : base(player, manager, stateMachine, data, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        started = false;
        timeSlowables = new();
    }

    override public void Exit()
    {
        base.Exit();

        started = false;
        manager.BulletTimeEffectObj.SetActive(false);
        ReleaseSlowables();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && !started && manager.CurrentEnergy >= data.bulletTimeRangedCost)
        {
            manager.DecreaseEnergy(data.bulletTimeRangedCost);
            player.InputHandler.UseTimeSkillInput();
            started = true;
            startTime = Time.time;
            manager.BulletTimeEffectObj.SetActive(true);
        }

        if(started && Time.time > startTime + data.bulletTimeRangedDuration)
        {
            started = false;
            manager.BulletTimeEffectObj.SetActive(false);
            ReleaseSlowables();
        }

        if (started)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(player.transform.position, data.bulletTimeRangedRadius, Vector2.zero, 0f, data.whatIsInteractable);

            if(hit.Length > 0)
            {
                List<ITimeSlowable> t_slowables = new();

                foreach (RaycastHit2D h in hit)
                {
                    if (h.collider.gameObject.TryGetComponent(out ITimeSlowable slowable))
                    {
                        t_slowables.Add(slowable);
                        if (!timeSlowables.Contains(slowable))
                        {
                            slowable.DoTimeSlow();
                            timeSlowables.Add(slowable);
                        }
                    }
                }


                foreach (ITimeSlowable slowable in timeSlowables.ToList())
                {
                    if (!t_slowables.Contains(slowable))
                    {
                        slowable.EndTimeSlow();
                        timeSlowables.Remove(slowable);
                    }
                }
            }
        }
    }

    private void ReleaseSlowables()
    {
        foreach (ITimeSlowable slowable in timeSlowables)
        {
            slowable.EndTimeSlow();
        }

        timeSlowables.Clear();
    }


}
