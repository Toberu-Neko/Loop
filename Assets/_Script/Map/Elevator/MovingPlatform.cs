using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, ITimeSlowable, ITimeStopable
{
    [SerializeField] private MovementStyle movementStyle;
    private enum MovementStyle
    {
        Constant,
        Circular,
        AutoTrigger,
        PressETrigger
    }
    private bool canMove;
    [SerializeField] private float delayTime;

    [SerializeField] private float speed;
    [SerializeField] private int startPoint;
    [SerializeField] private Transform[] points;


    [SerializeField] private Transform originalParent;
    [SerializeField] private Transform motherTransform;
    private int count;
    private bool reverse;
    private Collider2D playerCollider;
    private bool isDeactvating;

    private bool timeStop;
    private bool timeSlow;

    private void Awake()
    {
        transform.position = points[startPoint].position;
        count = startPoint;
        reverse = false;
        isDeactvating = false;
    }

    private void OnEnable()
    {
        timeStop = false;
        timeSlow = false;

        GameManager.Instance.OnAllTimeSlowStart += DoTimeSlow;
        GameManager.Instance.OnAllTimeSlowEnd += EndTimeSlow;
        GameManager.Instance.OnAllTimeStopStart += DoTimeStop;
        GameManager.Instance.OnAllTimeStopEnd += EndTimeStop;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnAllTimeSlowStart -= DoTimeSlow;
        GameManager.Instance.OnAllTimeSlowEnd -= EndTimeSlow;
        GameManager.Instance.OnAllTimeStopStart -= DoTimeStop;
        GameManager.Instance.OnAllTimeStopEnd -= EndTimeStop;
        
    }

    private void Update()
    {
        switch (movementStyle)
        {
            case MovementStyle.Constant:
                ConstantMovement();
                break;
            case MovementStyle.AutoTrigger:
                TriggerMovement();
                break;
            case MovementStyle.Circular:
                CircularMovement();
                break;
            case MovementStyle.PressETrigger:
                break;

        }
    }

    private void ConstantMovement()
    {
        if (Vector2.Distance(transform.position, points[count].position) < 0.01f)
        {
            CheckNextPoint();
        }

        Movement();
    }

    private void TriggerMovement()
    {
        if (Vector2.Distance(transform.position, points[count].position) < 0.01f)
        {
            canMove = false;
            CheckNextPoint();
        }

        if (canMove)
        {
            Movement();
        }
    }

    private void CircularMovement()
    {
        if (Vector2.Distance(transform.position, points[count].position) < 0.01f)
        {
            GoRound();
        }
        Movement();
    }

    private void Movement()
    {
        if (!timeStop)
        {
            if (timeSlow)
            {
                transform.position = Vector2.MoveTowards(transform.position, points[count].position, speed * Time.deltaTime * GameManager.Instance.TimeSlowMultiplier);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, points[count].position, speed * Time.deltaTime);
            }
        }
    }

    private void GoRound()
    {
        if(count == points.Length - 1)
        {
            count = 0;
        }
        else
        {
            count++;
        }
    }

    private void CheckNextPoint()
    {
        if (count == points.Length - 1)
        {
            reverse = true;
            count--;
            return;
        }
        else if (count == 0)
        {
            reverse = false;
            count++;
            return;
        }

        if (reverse)
        {
            count--;
        }
        else
        {
            count++;
        }
    }

    private void Deactivate()
    {
        isDeactvating = true;

        if(playerCollider!= null)
            playerCollider.transform.SetParent(null);

        Destroy(motherTransform.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(movementStyle == MovementStyle.AutoTrigger)
            {
                Invoke(nameof(SetCanMoveTrue), delayTime);
                CamManager.Instance.CameraShake();
            }

            playerCollider = collider;
            motherTransform.transform.SetParent(BaseTempParent.Instance.transform);
            collider.transform.SetParent(transform);
        }
    }

    private void SetCanMoveTrue()
    {
        CancelInvoke(nameof(SetCanMoveTrue));
        canMove = true;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CancelInvoke(nameof(SetCanMoveTrue));
            collision.transform.SetParent(null);
            if(originalParent != null)
            {
                motherTransform.transform.SetParent(originalParent);
            }
            else
            {
                Deactivate();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (points.Length > 1)
        {
            for(int i = 0; i < points.Length - 1; i++)
            {
                if(i + 1 == points.Length)
                {
                    Gizmos.DrawLine(points[i].position, points[0].position);
                }
                else
                {
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
                }
            }
            Gizmos.DrawLine(points[0].position, points[1].position);
        }
    }

    public void DoTimeSlow()
    {
        Debug.Log("DoTimeSlow");
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
