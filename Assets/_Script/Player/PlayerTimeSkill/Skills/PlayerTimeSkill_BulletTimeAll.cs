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

        SkillName = Data.bulletTimeAllSkillName;
        started = false;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && manager.CurrentEnergy >= Data.bulletTimeAllCost && !started)
        {
            started = true;
            startTime = Time.time;
            manager.DecreaseEnergy(Data.bulletTimeAllCost);

            player.InputHandler.UseTimeSkillInput();
            GameManager.Instance.StartAllTimeSlow(Data.bulletTimeAllDuration);
        }

        if(started && Time.time - startTime >= Data.bulletTimeAllDuration)
        {
            started = false;
        }
    }

}
