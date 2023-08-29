
using UnityEngine;

public class TimeSlow : CoreComponent, ITimeSlowable
{
    private GameManager gameManager;
    private Stats stats;

    protected override void Awake()
    {
        base.Awake();

        stats = core.GetCoreComponent<Stats>();
        gameManager = GameManager.Instance;
    }


    private void OnEnable()
    {
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

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
