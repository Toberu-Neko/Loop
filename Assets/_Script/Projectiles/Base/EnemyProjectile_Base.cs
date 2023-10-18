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

    protected virtual void Awake()
    {
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();
    }
    protected virtual void Update()
    {
        core.LogicUpdate();

        if (!HasHitGround && !countered)
        {
            movement.SetVelocity(speed, fireDirection);
        }
        if (!HasHitGround && countered)
        {
            movement.SetVelocity(counterVelocity);
        }
        if (HasHitGround || interected)
        {
            movement.SetVelocityZero();
        }

        startTime = stats.Timer(startTime);

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

    public virtual void Fire(Vector2 fireDirection, float speed, ProjectileDetails details)
    {
        this.fireDirection = fireDirection;
        this.speed = speed;
        this.details = details;
        whatIsTargetLayer = whatIsPlayer;
        startPos = transform.position;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, fireDirection);

        startTime = Time.time;
        transform.rotation = targetRotation;
        movement.SetGravityZero();
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
            countered = true;
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


            if (stats.IsTimeStopped)
            {
                if (facingDirection != direction)
                {
                    counterVelocity = movement.TimeStopVelocity * -4f;
                    movement.SetTimeStopVelocity(counterVelocity);
                    movement.Turn();
                }
                else
                {
                    movement.SetTimeStopVelocity(counterVelocity);
                }
            }

            else if (stats.IsTimeSlowed)
            {
                if (facingDirection != direction)
                {
                    counterVelocity = -movement.CurrentVelocity / GameManager.Instance.TimeSlowMultiplier;
                    movement.SetTimeSlowVelocity(counterVelocity / GameManager.Instance.TimeSlowMultiplier * 5f);
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
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (((1 << collider.gameObject.layer) & whatIsTargetLayer) != 0 && !HasHitGround && !interected)
        {
            interected = true;
            OnHitTargetAction?.Invoke(collider);
        }

        if (((1 << collider.gameObject.layer) & whatIsGround) != 0)
        {
            HasHitGround = true;
            
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
        HasHitGround = true;
        ObjectPoolManager.ReturnObjectToPool(gameObject);
    }
}
