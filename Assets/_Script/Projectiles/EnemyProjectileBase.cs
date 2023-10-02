using System;
using UnityEngine;

public class EnemyProjectileBase : MonoBehaviour, IKnockbackable, IFireable
{
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsPlayer;
    private LayerMask _whatIsPlayer;
    [SerializeField] protected Core core;
    protected Movement movement;
    protected Stats stats;

    protected event Action<Collider2D> OnAction;

    [SerializeField] private Animator anim;

    protected bool hasHitGround;
    protected bool countered;
    private bool interected = false;
    protected ProjectileDetails details;

    private Vector2 counterVelocity;
    private Vector2 fireDirection;

    protected virtual void Awake()
    {
        movement = core.GetCoreComponent<Movement>();
        stats = core.GetCoreComponent<Stats>();
    }
    protected virtual void Update()
    {
        core.LogicUpdate();

        if (!hasHitGround && !countered)
        {
            movement.SetVelocity(details.speed, fireDirection);
        }
        if (!hasHitGround && countered)
        {
            movement.SetVelocity(counterVelocity);
        }
        if (hasHitGround)
        {
            movement.SetVelocityZero();
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
        _whatIsPlayer = whatIsPlayer;

        hasHitGround = false;
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
    public void Fire(Vector2 fireDirection, ProjectileDetails details)
    {
        this.details = details;
        this.fireDirection = fireDirection;
        _whatIsPlayer = whatIsPlayer;

        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.right, fireDirection);

        transform.rotation = targetRotation;
        movement.SetGravityZero();
        movement.SetVelocity(details.speed, fireDirection);

        Invoke(nameof(ReturnToPool), 10f);
    }

    public void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true, bool forceKnockback = false)
    {
        if ((stats.IsTimeStopped || stats.IsTimeSlowed) && !countered)
        {
            countered = true;
            gameObject.layer = LayerMask.NameToLayer("PlayerAttack");
            _whatIsPlayer = LayerMask.GetMask("Damageable");

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _whatIsPlayer) != 0 && !hasHitGround && !interected)
        {
            Debug.Log("Interected!");
            interected = true;
            OnAction?.Invoke(collision);
        }

        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            hasHitGround = true;
            movement.SetVelocityZero();

            CancelInvoke(nameof(ReturnToPool));
            Invoke(nameof(ReturnToPool), 5f);
        }

    }

    protected void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));
        if (!stats.InCombat || interected || hasHitGround)
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
        else
        {
            Invoke(nameof(ReturnToPool), 3f);
        }
    }
}
