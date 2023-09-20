using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private MovementStyle movementStyle;
    private enum MovementStyle
    {
        Constant,
        Circular,
        Trigger
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

    private void Awake()
    {
        transform.position = points[startPoint].position;
        count = startPoint;
        reverse = false;
        isDeactvating = false;
    }

    private void Update()
    {
        foreach (var item in points)
        {
            if (item == null && !isDeactvating)
            {
                Deactivate();
                return;
            }
        }

        if (movementStyle == MovementStyle.Constant)
        {
            ConstantMovement();
        }
        else if(movementStyle == MovementStyle.Trigger)
        {
            TriggerMovement();
        }
        else if(movementStyle == MovementStyle.Circular)
        {
            CircularMovement();
        }


    }

    private void ConstantMovement()
    {
        if (Vector2.Distance(transform.position, points[count].position) < 0.01f)
        {
            CheckNextPoint();
        }

        transform.position = Vector2.MoveTowards(transform.position, points[count].position, speed * Time.deltaTime);
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
            transform.position = Vector2.MoveTowards(transform.position, points[count].position, speed * Time.deltaTime);
        }
    }

    private void CircularMovement()
    {
        if (Vector2.Distance(transform.position, points[count].position) < 0.01f)
        {
            GoRound();
        }

        transform.position = Vector2.MoveTowards(transform.position, points[count].position, speed * Time.deltaTime);
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
        playerCollider?.transform.SetParent(null);

        Destroy(motherTransform.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(movementStyle == MovementStyle.Trigger)
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
}
