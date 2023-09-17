using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkill_BulletTimeAll : PlayerTimeSkillBase
{
    private bool started;
    private float startTime;
    public PlayerTimeSkill_BulletTimeAll(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName) : base(player, manager, stateMachine, data, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        started = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && manager.CurrentEnergy >= data.bulletTimeAllCost && !started)
        {
            started = true;
            startTime = Time.time;
            manager.DecreaseEnergy(data.bulletTimeAllCost);

            player.InputHandler.UseTimeSkillInput();
            GameManager.Instance.StartAllTimeSlow(data.bulletTimeAllDuration);
        }

        if(started && Time.time - startTime >= data.bulletTimeAllDuration)
        {
            started = false;
        }
    }

}
