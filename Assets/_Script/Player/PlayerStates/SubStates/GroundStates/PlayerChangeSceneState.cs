using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSceneState : PlayerState
{
    private bool canChangeState;
    private bool firstTime;
    public PlayerChangeSceneState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        firstTime = true;
    }
    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();
        Movement.SetRBKinematic();

        canChangeState = false;
    }

    public override void Exit()
    {
        base.Exit();

        Movement.SetRBDynamic();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();

        if (canChangeState)
        {
            if (firstTime)
            {
                firstTime = false;
                stateMachine.ChangeState(player.TurnOnState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }

    public void SetCanChangeStateTrue()
    {
        canChangeState = true;
    }
}
