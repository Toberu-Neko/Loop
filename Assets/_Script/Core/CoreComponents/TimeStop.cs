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

    private void HandleTimeStop()
    {
        stats.SetIsStoppedTrue();
    }

    private void HandleTimeStart()
    {
        stats.SetIsStoppedFalse();
    }
}
