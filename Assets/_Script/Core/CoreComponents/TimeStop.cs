using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : CoreComponent, ITimeStopable
{
    private Stats stats;

    private void Start()
    {
        stats = core.GetCoreComponent<Stats>();

        GameManager.Instance.OnAllTimeStopEnemy += HandleTimeStop;
        GameManager.Instance.OnAllTimeStartEnemy += HandleTimeStart;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnAllTimeStopEnemy -= HandleTimeStop;
        GameManager.Instance.OnAllTimeStartEnemy -= HandleTimeStart;
    }

    public void DoTimeStop(float stopTime)
    {
        stats.SetIsStoppedTrue();

        CancelInvoke(nameof(HandleTimeStart));
        Invoke(nameof(HandleTimeStart), stopTime);
    }

    private void HandleTimeStop()
    {
        stats.SetIsStoppedFalse();
    }
    private void HandleTimeStart()
    {
        stats.SetIsStoppedFalse();
    }
}
