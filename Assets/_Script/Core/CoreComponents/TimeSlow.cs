
using UnityEngine;

public class TimeSlow : CoreComponent, ITimeSlowable
{
    private Stats stats;

    protected override void Awake()
    {
        base.Awake();

        stats = core.GetCoreComponent<Stats>();
    }


    private void OnEnable()
    {
        GameManager.Instance.OnAllTimeSlowStart += DoTimeSlow;
        GameManager.Instance.OnAllTimeSlowEnd += EndTimeSlow;

        if (GameManager.Instance.TimeSlowAll)
        {
            DoTimeSlow();
        }
    }

    private void OnDisable()
    {
        GameManager.Instance.OnAllTimeSlowStart -= DoTimeSlow;
        GameManager.Instance.OnAllTimeSlowEnd -= EndTimeSlow;

        if(stats.IsTimeSlowed)
        {
            EndTimeSlow();
        }
    }


    public void DoTimeSlow()
    {
        stats.SetTimeSlowTrue();
    }

    public void EndTimeSlow()
    {
        stats.SetTimeSlowFalse();
    }
}
