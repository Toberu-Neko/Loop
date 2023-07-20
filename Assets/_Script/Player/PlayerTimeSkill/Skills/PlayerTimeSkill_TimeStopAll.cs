using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkill_TimeStopAll : PlayerTimeSkillBase
{
    public PlayerTimeSkill_TimeStopAll(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName) : base(player, manager, stateMachine, data, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && !GameManager.Instance.TimeStopEnemy && manager.CurrentEnergy >= data.timeStopAllCostPerSecond * Time.deltaTime)
        {
            player.InputHandler.UseTimeSkillInput();
            GameManager.Instance.SetTimeStopEnemyTrue();
        }
        else if (player.InputHandler.TimeSkillInput && GameManager.Instance.TimeStopEnemy)
        {
            player.InputHandler.UseTimeSkillInput();
            GameManager.Instance.SetTimeStopEnemyFalse();
        }

        if(GameManager.Instance.TimeStopEnemy)
        {
            manager.DecreaseEnergy(data.timeStopAllCostPerSecond * Time.deltaTime);
        }
    }
}
