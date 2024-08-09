using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If a object has this component, it can be stopped in time.
/// </summary>
public class TimeStop : CoreComponent, ITimeStopable
{
    private Stats stats;

    protected override void Awake()
    {
        base.Awake();
        stats = core.GetCoreComponent<Stats>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnAllTimeStopStart += HandleTimeStopStart;
        GameManager.Instance.OnAllTimeStopEnd += EndTimeStop;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnAllTimeStopStart -= HandleTimeStopStart;
        GameManager.Instance.OnAllTimeStopEnd -= EndTimeStop; 
        
        CancelInvoke(nameof(EndTimeStop));
    }

    public void DoTimeStopWithTime(float stopTime)
    {
        stats.SeTimeStopTrue();

        CancelInvoke(nameof(EndTimeStop));
        Invoke(nameof(EndTimeStop), stopTime);
    }

    public void DoTimeStop()
    {
        stats.SeTimeStopTrue();
    }

    private void HandleTimeStopStart()
    {
        stats.SeTimeStopTrue();
    }

    public void EndTimeStop()
    {
        stats.SetTimeStopFalse();
    }
}
