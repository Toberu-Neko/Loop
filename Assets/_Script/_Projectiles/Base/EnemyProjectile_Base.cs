using System;
using UnityEngine;

public class EnemyProjectile_Base : MonoBehaviour, IKnockbackable, IFireable
{
    [Header("Base")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    protected LayerMask whatIsTargetLayer;
    [SerializeField] protected Core core;
    [SerializeField] protected SpriteRenderer SR;
    protected Movement movement;
    protected Stats stats;

    protected event Action<Collider2D> OnHitTargetAction;
    protected event Action OnHitGroundAction;
    protected event Action OnDuration;

    [SerializeField] private Animator anim;

    public bool HasHitGround { get; protected set; }
    protected bool countered;
    protected float startTime;
    private bool interected;
    protected ProjectileDetails details;

    protected float speed;
    protected Vector2 startPos;
    private Vector2 counterVelocity;
    protected Vector2 fireDirection;

    [SerializeField] private CounterType counterType;
    private MovementType movementType;
    private enum CounterType
    {
        Flip,
        Relative
    }

    private enum MovementType
    {
        Idle,
        Move,
        Counter,
        Hitted
    }

    protected virtual void Awake()
    {
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();
    }

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        core.LogicUpdate();

        if (movementType == MovementType.Move)
        {
            startTime = stats.Timer(startTime);
            movement.SetVelocity(speed, fireDirection);
        }
        if (movementType == MovementType.Counter)
        {
            startTime = stats.Timer(startTime);
            movement.SetVelocity(counterVelocity);
        }
        if (movementType == MovementType.Hitted)
        {
            movement.SetVelocityZero();
        }


        if(Time.time >= startTime + details.duration)
        {
            HandleDuration();
        }
    }

    protected virtual void LateUpdate()
    {
        core.LateLogicUpdate();
    }

    protected virtual void FixedUpdate()
    {
        core.PhysicsUpdate();
    }

    protected virtual void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("EnemyAttack");
        whatIsTargetLayer = whatIsPlayer;

        movementType = MovementType.Idle;
        HasHitGround = false;
        interected = false;
        countered = false;

        stats.OnTimeSlowStart += HandleChangeAnimSlow;
        stats.OnTimeSlowEnd += HandleChangeAnimOrigin;
        stats.OnTimeStopStart += HandleChangeAnimSlow;
        stats.OnTimeStopEnd += HandleChangeAnimOrigin;
    }

    protected virtual void OnDisable()
    {
        anim.SetBool("timeSlow", false);

        stats.OnTimeStopStart -= HandleChangeAnimSlow;
        stats.OnTimeStopEnd -= HandleChangeAnimOrigin;
        stats.OnTimeSlowStart -= HandleChangeAnimSlow;
        stats.OnTimeSlowEnd -= HandleChangeAnimOrigin;
    }

    private void HandleChangeAnimSlow()
    {
        anim.SetBool("timeSlow", true);
    }

    private void HandleChangeAnimOrigin()
    {
        anim.SetBool("timeSlow", false);
    }

    public virtual void Init(Vector2 fireDirection, float speed, ProjectileDetails details)
    {
        this.fireDirection = fireDirection;
        this.speed = speed;
        this.details = details;
    }
    public virtual void Fire()
    {
        movementType = MovementType.Move;
        whatIsTargetLayer = whatIsPlayer;
        startPos = transform.position;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, fireDirection);

        startTime = Time.time;
        transform.rotation = targetRotation;
        movement.SetVelocity(speed, fireDirection);
    }


    public virtual void HandlePerfectBlock()
    {
        ReturnToPool();
    }

    public virtual void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        if ((stats.IsTimeStopped || stats.IsTimeSlowed) && !countered && !interected)
        {
            CamManager.Instance.CameraShake(2.5f);
            countered = true;
            movementType = MovementType.Counter;
            gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
            whatIsTargetLayer = LayerMask.GetMask("Damageable");

            startTime = Time.time; 

            int direction = transform.position.x < damagePosition.x ? -1 : 1;
            int facingDirection;

            if (fireDirection.x >= 0f)
            {
                facingDirection = 1;
            }
            else
            {
                facingDirection = -1;
            }


            if(counterType == CounterType.Flip)
            {
                if (stats.IsTimeStopped)
                {
                    if (facingDirection != direction)
                    {
                        counterVelocity = -4f * -speed * fireDirection;
                        movement.SetTimeStopVelocity(counterVelocity);
                        movement.Turn();
                    }
                    else
                    {
                        counterVelocity = movement.TimeStopVelocity * 4f;
                        movement.SetTimeStopVelocity(counterVelocity);
                    }
                }

                else if (stats.IsTimeSlowed)
                {
                    if (facingDirection != direction)
                    {
                        counterVelocity = -speed * fireDirection;
                        movement.SetTimeSlowVelocity(counterVelocity);
                        movement.Turn();
                    }
                    else
                    {
                        counterVelocity = movement.TimeSlowVelocity * 4f;
                        movement.SetVelocity(movement.CurrentVelocity);
                        movement.SetTimeSlowVelocity(counterVelocity);
                    }
                }
            }
            else if (counterType == CounterType.Relative)
            {
                Vector2 counterDirection = ((Vector2)transform.position - damagePosition).normalized;
                if (stats.IsTimeStopped)
                {
                    counterVelocity = -4f * speed * counterDirection;
                    movement.SetTimeStopVelocity(counterVelocity);

                    DeterminTurn(damagePosition, counterDirection);
                }

                else if (stats.IsTimeSlowed)
                {
                    counterVelocity = speed * counterDirection;
                    movement.SetTimeSlowVelocity(counterVelocity);

                    DeterminTurn(damagePosition, counterDirection);
                }
            }
        }
    }

    private void DeterminTurn(Vector2 damagePos, Vector2 counterDirection)
    {
        float t_angle;
        if (damagePos.x > transform.position.x)
        {
            t_angle = Vector2.Angle(fireDirection, -counterDirection) + 180f;
        }
        else
        {
            t_angle = Vector2.Angle(fireDirection, counterDirection);
        }

        movement.Turn(t_angle);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & whatIsTargetLayer) != 0 && !HasHitGround && !interected)
        {
            interected = true;
            movementType = MovementType.Hitted;
            OnHitTargetAction?.Invoke(collider);
        }

        if (((1 << collider.gameObject.layer) & whatIsGround) != 0)
        {
            HasHitGround = true;
            
            movementType = MovementType.Hitted;
            movement.SetVelocityZero();

            OnHitGroundAction?.Invoke();
        }
    }

    private void HandleDuration()
    {
        OnDuration?.Invoke();
    }

    protected virtual void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));

        if (gameObject.activeInHierarchy)
        {
            HasHitGround = true;
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }

    }
}
