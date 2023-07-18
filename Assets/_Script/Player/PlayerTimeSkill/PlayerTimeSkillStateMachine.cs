using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimeSkillStateMachine
{
    public PlayerTimeSkillBase CurrentState { get; private set; }

    public void Initialize(PlayerTimeSkillBase startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerTimeSkillBase newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        
        // Debug.Log(newState.ToString());
        CurrentState.Enter();
    }
}
