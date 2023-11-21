using System;
using UnityEngine;

[Serializable]
public class EnemyStateMachine
{
    public EnemyState CurrentState { get; private set; }
    public EnemyState PreviousState { get; private set; }
    public event Action OnChangeState;
    private bool canChangeState = true;
    [SerializeField] private bool debugCanChangeState = true;
    public void Initialize(EnemyState startingState)
    {
        CurrentState = startingState;
        PreviousState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if (canChangeState && debugCanChangeState)
        {
            if(CurrentState == null)
            {
                Debug.LogWarning("Current state is null, this should not happen.");
            }
            else
            {
                CurrentState.Exit();
            }
            PreviousState = CurrentState;
            CurrentState = newState;

            OnChangeState?.Invoke();

            // Debug.Log(newState.ToString());

            CurrentState.Enter();
        }
    }

    public void SetCanChangeState(bool canChangeState)
    {
        this.canChangeState = canChangeState;
    }
}
