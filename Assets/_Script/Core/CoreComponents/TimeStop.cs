using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : CoreComponent
{
    private Movement movement;
    private Stats stats;

    private void Start()
    {
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();

        GameManager.Instance.OnTimeStopEnemy += HandleTimeStop;
        GameManager.Instance.OnTimeStartEnemy += HandleTimeStart;
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
