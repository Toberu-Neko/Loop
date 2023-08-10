using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : CoreComponent, ITimeStopable
{
    private Stats stats;

    private void Start()
    {
        stats = core.GetCoreComponent<Stats>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnAllTimeStopStart += HandleTimeStopStart;
        GameManager.Instance.OnAllTimeStopEnd += HandleTimeStopEnd;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnAllTimeStopStart -= HandleTimeStopStart;
        GameManager.Instance.OnAllTimeStopEnd -= HandleTimeStopEnd;
    }

    public void DoTimeStop(float stopTime)
    {
        stats.SeTimeStopTrue();

        CancelInvoke(nameof(HandleTimeStopEnd));
        Invoke(nameof(HandleTimeStopEnd), stopTime);
    }

    private void HandleTimeStopStart()
    {
        stats.SeTimeStopTrue();
    }
    private void HandleTimeStopEnd()
    {
        stats.SetTimeStopFalse();
    }
}
