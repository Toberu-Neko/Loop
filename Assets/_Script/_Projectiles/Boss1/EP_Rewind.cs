using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EP_Rewind : EnemyProjectile_Base, IRewindable
{
    [SerializeField] private GameObject bookmarkPrefab;
    [SerializeField] private Collider2D col;
    [SerializeField] private GameObject dangerText;
    private GameObject bookmarkObj;
    private bool startRewind = false;
    private bool fire = false;
    private float startRewindTime;
    private bool interacted;

    public override void Fire(Vector2 fireDirection, float speed, ProjectileDetails details)
    {
        base.Fire(fireDirection, speed, details);

        fire = true;
        interacted = false;
        if (HasHitGround)
        {
            HasHitGround = false;
        }
        Invoke(nameof(SetHasGrounded), 2f);
    }

    public override void HandlePerfectBlock()
    {
        base.HandlePerfectBlock();
    }

    public override void Knockback(Vector2 angle, float force, Vector2 damagePosition, bool blockable = true)
    {
        ReturnToPool();
    }

    public void Rewind(bool doRewind = true)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (doRewind)
        {
            interacted = false;
            col.enabled = true;
            startRewind = true;
            HasHitGround = false;
            startRewindTime = Time.time;
        }
        else
        {
            ReturnToPool();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CancelInvoke(nameof(SetHasGrounded));
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        fire = false;
        startRewind = false;
        HasHitGround = false;
        bookmarkObj = null;
        dangerText.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsTargetLayer) != 0 && !HasHitGround)
        {
            HandleHitTarget(collision);
        }

        if (((1 << collision.gameObject.layer) & whatIsGround) != 0)
        {
            HasHitGround = true;
            CancelInvoke(nameof(SetHasGrounded));
            movement.SetVelocityZero();
        }
    }

    protected override void Update()
    {
        core.LogicUpdate();
        startTime = stats.Timer(startTime);

        if (startRewind)
        {
            movement.SetVelocity(speed * -fireDirection);

            if (Vector2.Distance((Vector2)transform.position, startPos) < 0.1f)
            {
                ReturnToPool();
            }

            if(startRewindTime >= Time.time + 3f)
            {
                ReturnToPool();
            }
        }
        else if (!HasHitGround && fire)
        {
            movement.SetVelocity(speed, fireDirection);
        }
        else
        {
            movement.SetVelocityZero();
        }
    }

    public void SetDetails(ProjectileDetails details, bool doRewind)
    {
        this.details = details;

        if (details.combatDetails.blockable)
        {
            SR.color = Color.yellow;
        }
        else
        {
            SR.color = Color.red;
            dangerText.SetActive(true);
        }

        if (doRewind)
        {
            bookmarkObj = ObjectPoolManager.SpawnObject(bookmarkPrefab, transform.position, transform.rotation);
            bookmarkObj.GetComponent<SpriteRenderer>().sprite = SR.sprite;
        }
    }

    private void SetHasGrounded()
    {
        if (!HasHitGround)
        {
            col.enabled = false;
            HasHitGround = true;
        }
    }


    private void HandleHitTarget(Collider2D collider)
    {
        if(interacted)
            return;

        interacted = true;
        if (collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(details.combatDetails.damageAmount, transform.position, details.combatDetails.blockable);
        }
        if (collider.TryGetComponent(out IKnockbackable knockbackable))
        {
            knockbackable.Knockback(details.combatDetails.knockbackAngle, details.combatDetails.knockbackStrength, transform.position, details.combatDetails.blockable);
        }
        if (collider.TryGetComponent(out IStaminaDamageable staminaDamageable))
        {
            staminaDamageable.TakeStaminaDamage(details.combatDetails.staminaDamageAmount, transform.position, details.combatDetails.blockable);
        }
    }

    protected override void ReturnToPool()
    {
        base.ReturnToPool();

        if(bookmarkObj != null)
        {
            ObjectPoolManager.ReturnObjectToPool(bookmarkObj);
        }   
    }
}
