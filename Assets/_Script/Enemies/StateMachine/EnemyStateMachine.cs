using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }
    private bool canChangeState = true;

    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState newState)
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
