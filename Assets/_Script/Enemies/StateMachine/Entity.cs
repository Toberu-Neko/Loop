using UnityEngine;
using System.Linq;
using System;

public class Entity : MonoBehaviour
{
    public EnemyStateMachine StateMachine { get; private set; }
    [SerializeField] private D_Entity EntityData;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Core Core { get; private set; }
    protected Movement movement;
    public Stats Stats { get; private set; }
    private Combat combat;
    private CollisionSenses collisionSenses;

    public Animator Anim { get; private set; }
    private float animSpeed;
    private WeaponAttackDetails collisionAttackDetails;
    public bool SkillCollideDamage { get; private set; }

    public event Action OnDefeated;


    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        movement = Core.GetCoreComponent<Movement>();
        Stats = Core.GetCoreComponent<Stats>();
        combat = Core.GetCoreComponent<Combat>();
        collisionSenses = Core.GetCoreComponent<CollisionSenses>();

        Anim = GetComponent<Animator>();
        animSpeed = 1f;
        collisionAttackDetails = EntityData.collisionAttackDetails;

        movement.OrginalGravityScale = EntityData.gravityScale;
        StateMachine = new();
    }

    protected virtual void OnEnable()
    {
        Stats.OnTimeStopStart += HandleOnTimeStop;
        Stats.OnTimeStopEnd += HandleOnTimeStart;
        Stats.OnTimeSlowStart += HandleTimeSlowStart;
        Stats.OnTimeSlowEnd += HandleTimeSlowEnd;
        Stats.Health.OnCurrentValueZero += HandleHealthZero;

        Stats.Health.Init();
        Stats.Stamina.Init();

    }

    protected virtual void Start()
    {

    }

    protected virtual void OnDisable()
    {
        Stats.OnTimeStopStart -= HandleOnTimeStop;
        Stats.OnTimeStopEnd -= HandleOnTimeStart;
        Stats.OnTimeSlowStart -= HandleTimeSlowStart;
        Stats.OnTimeSlowEnd -= HandleTimeSlowEnd;
        Stats.Health.OnCurrentValueZero -= HandleHealthZero;

        Anim.speed = 1f;
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        Anim.SetFloat("yVelocity", movement.RB.velocity.y);
    }

    private void LateUpdate()
    {
        Core.LateLogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        Core.PhysicsUpdate();

        StateMachine.CurrentState.DoChecks();
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual void OnDrawGizmos()
    {
    }

    private void HandleTimeSlowStart()
    {
        Anim.speed *= GameManager.Instance.TimeSlowMultiplier;
    }
    private void HandleTimeSlowEnd()
    {
        Anim.speed /= GameManager.Instance.TimeSlowMultiplier;
    }

    private void HandleOnTimeStop()
    {
        Anim.speed = 0;
        StateMachine.SetCanChangeState(false);
    }

    private void HandleOnTimeStart()
    {
        Anim.speed = animSpeed;
        StateMachine.SetCanChangeState(true);
    }

    private void AnimationActionTrigger()
    {
        StateMachine.CurrentState.AnimationActionTrigger();
    }
    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public void SetFacingDirection(int dir)
    {
        if(movement == null)
        {
            Debug.LogError("movement is null");
            return;
        }
        movement.CheckIfShouldFlip(dir);
    }

    public Vector2 GetPosition()
    {
        return (Vector2)transform.position;
    }

    public void DoDamageToDamageList(float damageAmount, float damageStaminaAmount, Vector2 knockBackAngle, float knockBackForce, bool blockable = true)
    {
        if (combat.DetectedDamageables.Count > 0)
        {
            foreach (IDamageable damageable in combat.DetectedDamageables.ToList())
            {
                damageable.Damage(damageAmount, GetPosition(), blockable);
            }
        }

        if (combat.DetectedKnockbackables.Count > 0)
        {
            foreach (IKnockbackable knockbackable in combat.DetectedKnockbackables.ToList())
            {
                knockbackable.Knockback(knockBackAngle, knockBackForce, movement.ParentTransform.position, blockable);
            }
        }

        if (combat.DetectedStaminaDamageables.Count > 0)
        {
            foreach (IStaminaDamageable staminaDamageable in combat.DetectedStaminaDamageables.ToList())
            {
                staminaDamageable.TakeStaminaDamage(damageStaminaAmount, GetPosition(), blockable);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && Stats.Health.CurrentValue > 0f)
        {

            if (collision.gameObject.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(collisionAttackDetails.knockbackAngle, collisionAttackDetails.knockbackForce, movement.ParentTransform.position, false);
            }
            if(collision.gameObject.TryGetComponent(out IDamageable damageable) && (EntityData.collideDamage || SkillCollideDamage))
            {
                damageable.Damage(collisionAttackDetails.damageAmount, GetPosition(), false);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && Stats.Health.CurrentValue > 0f)
        {
            int direction;
            if (collision.gameObject.transform.position.x > GetPosition().x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            if (collisionSenses.WallFrontLong)
            {
                direction = -movement.FacingDirection;
            }

            if(collisionSenses.WallBackLong)
            {
                direction = movement.FacingDirection;
            }

            if (collision.gameObject.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(collisionAttackDetails.knockbackAngle, collisionAttackDetails.knockbackForce, movement.ParentTransform.position, false);
            }
            if (collision.gameObject.TryGetComponent(out IDamageable damageable) && (EntityData.collideDamage || SkillCollideDamage))
            {
                damageable.Damage(collisionAttackDetails.damageAmount, GetPosition(), false);
            }
        }
    }

    public void SetSkillCollideDamage(bool value) => SkillCollideDamage = value;

    private void HandleHealthZero()
    {
        OnDefeated?.Invoke();
    }

    public Sprite GetCurrentSprite()
    {
        return spriteRenderer.sprite;
    }

    /*
    public void LoadData(GameData data)
    {
        data.defeatedEnemies.TryGetValue(ID, out isDefeated);

        if(isDefeated)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.defeatedEnemies.ContainsKey(ID))
        {
            data.defeatedEnemies.Remove(ID);
        }
        data.defeatedEnemies.Add(ID, isDefeated);
    }*/
}
