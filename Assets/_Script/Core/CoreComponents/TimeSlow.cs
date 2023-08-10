
using UnityEngine;

public class TimeSlow : CoreComponent, ITimeSlowable
{
    private GameManager gameManager;
    private Stats stats;

    private void OnEnable()
    {
        stats = core.GetCoreComponent<Stats>();
        gameManager = GameManager.Instance;

        gameManager.OnAllTimeSlowStart += DoTimeSlow;
        gameManager.OnAllTimeSlowEnd += EndTimeSlow;

        if (gameManager.TimeSlowAll)
        {
            DoTimeSlow();
        }
    }
    private void OnDisable()
    {
        gameManager.OnAllTimeSlowStart -= DoTimeSlow;
        gameManager.OnAllTimeSlowEnd -= EndTimeSlow;
    }

    public void DoTimeSlow()
    {
        stats.SetTimeSlowTrue();
    }

    public void EndTimeSlow()
    {
        stats.SetTimeSlowFalse();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
