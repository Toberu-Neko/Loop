using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRewindState : PlayerAbilityState
{
    public PlayerRewindState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
}
