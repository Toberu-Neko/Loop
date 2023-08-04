
public class TimeSlow : CoreComponent, ITimeSlowable
{
    private Stats stats;

    private void Start()
    {
        stats = core.GetCoreComponent<Stats>();

    }
    private void OnDisable()
    {
        
    }

    public void DoTimeSlow()
    {

    }
}
