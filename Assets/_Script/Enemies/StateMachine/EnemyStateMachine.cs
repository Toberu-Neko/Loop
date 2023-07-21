using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public State CurrentState { get; private set; }
    private bool canChangeState = true;

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(State newState)
    {
        if (canChangeState)
        {
            CurrentState.Exit();
            CurrentState = newState;

            // Debug.Log(newState.ToString());

            CurrentState.Enter();
        }
    }

    public void SetCanChangeState(bool canChangeState)
    {
        this.canChangeState = canChangeState;
    }
}
