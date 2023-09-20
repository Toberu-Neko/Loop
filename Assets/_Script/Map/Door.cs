using UnityEngine;

public class Door : MonoBehaviour, ITimeSlowable, ITimeStopable
{
    [SerializeField] private Transform downTransform;
    [SerializeField] private Transform upTransform;
    [SerializeField] private float upSpeed;
    [SerializeField] private float downSpeed;


    [SerializeField] private bool goingUp;
    [SerializeField] private DoorTrigger[] doorTriggers;
    private bool goingDown;

    private bool timeStop;
    private bool timeSlow;

    private void Awake()
    {
        transform.position = downTransform.position;
        timeSlow = false;
        timeStop = false;
        goingUp = false;
        goingDown = false;
    }

    private void Start()
    {
        GameManager.Instance.OnAllTimeSlowStart += DoTimeSlow;
        GameManager.Instance.OnAllTimeSlowEnd += EndTimeSlow;
        GameManager.Instance.OnAllTimeStopStart += DoTimeStop;
        GameManager.Instance.OnAllTimeStopEnd += EndTimeStop;
    }


    private void OnEnable()
    {
        foreach(var trigger in doorTriggers)
        {
            trigger.OnDoorTriggered += HandleDoorTriggered;
        }
    }

    private void OnDisable()
    {
        foreach (var trigger in doorTriggers)
        {
            trigger.OnDoorTriggered -= HandleDoorTriggered;
        }

        GameManager.Instance.OnAllTimeSlowStart -= DoTimeSlow;
        GameManager.Instance.OnAllTimeSlowEnd -= EndTimeSlow;
        GameManager.Instance.OnAllTimeStopStart -= DoTimeStop;
        GameManager.Instance.OnAllTimeStopEnd -= EndTimeStop;
    }

    private void Update()
    {
        if (goingUp)
        {
            Movement(upTransform.position, upSpeed);
        }
        else if (goingDown)
        {
            Movement(downTransform.position, downSpeed);
        }

        if(Vector2.Distance(transform.position, upTransform.position) < 0.01f && goingUp)
        {
            goingUp = false;
            goingDown = true;
        }

        if(Vector2.Distance(transform.position, downTransform.position) < 0.01f && goingDown)
        {
            goingDown = false;
            CamManager.Instance.CameraShake();
        }
    }

    private void HandleDoorTriggered()
    {
        if(!goingDown && !goingUp)
            goingUp = true;
    }

    private void Movement(Vector2 position, float speed)
    {
        if (!timeStop)
        {
            if (timeSlow)
            {
                transform.position = Vector2.MoveTowards(transform.position, position, speed * Time.deltaTime * GameManager.Instance.TimeSlowMultiplier);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, position, speed * Time.deltaTime);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(downTransform.position, upTransform.position);
    }


    public void DoTimeSlow()
    {
        timeSlow = true;
    }

    public void EndTimeSlow()
    {
        timeSlow = false;
    }
    public void DoTimeStop()
    {
        timeStop = true;
    }
    public void DoTimeStopWithTime(float stopTime)
    {
        timeStop = true;

        CancelInvoke(nameof(EndTimeStop));
        Invoke(nameof(EndTimeStop), stopTime);

    }
    public void EndTimeStop()
    {
        timeStop = false;
    }
}
