using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegenState : PlayerAbilityState
{
    public PlayerRegenState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityZero();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement.SetVelocityZero();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (PlayerInventoryManager.Instance.ConsumablesInventory["Medkit"].itemCount > 0)
        {
            PlayerInventoryManager.Instance.RemoveConsumableItem("Medkit");
        }
        else
        {
            player.TimeSkillManager.DecreaseEnergy(playerData.regenCost);
        }
        Stats.Health.Increase(playerData.regenAmount);

        isAbilityDone = true;
    }
}
