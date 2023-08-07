using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkill_TimeStopAll : PlayerTimeSkillBase
{

    public PlayerTimeSkill_TimeStopAll(Player player, PlayerTimeSkillManager manager, PlayerTimeSkillStateMachine stateMachine, PlayerTimeSkillData data, string animBoolName) : base(player, manager, stateMachine, data, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        GameManager.Instance.OnChangeSceneFinished += HandleChangeScene;
    }
    public override void Exit()
    {
        base.Exit();

        GameManager.Instance.SetTimeStopEnemyFalse();
        GameManager.Instance.OnChangeSceneFinished -= HandleChangeScene;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.TimeSkillInput && !GameManager.Instance.TimeStopAll && manager.CurrentEnergy >= data.timeStopAllCostPerSecond * Time.deltaTime)
        {
            player.InputHandler.UseTimeSkillInput();
            GameManager.Instance.SetTimeStopEnemyTrue();
        }
        else if (player.InputHandler.TimeSkillInput && GameManager.Instance.TimeStopAll)
        {
            player.InputHandler.UseTimeSkillInput();
            GameManager.Instance.SetTimeStopEnemyFalse();
        }

        if(GameManager.Instance.TimeStopAll)
        {
            manager.DecreaseEnergy(data.timeStopAllCostPerSecond * Time.deltaTime);
        }
    }

    private void HandleChangeScene()
    {
        GameManager.Instance.SetTimeStopEnemyFalse();
    }
}
