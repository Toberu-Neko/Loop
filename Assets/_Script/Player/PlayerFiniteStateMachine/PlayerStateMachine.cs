using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    /// <summary>
    /// Use this to call the first state. It will not call Exit() on the previous(null) state.
    /// </summary>
    /// <param name="startingState"></param>
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    /// <summary>
    /// Use this to change the state. 
    /// It will call Exit() on the previous state and Enter() on the new state.
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
