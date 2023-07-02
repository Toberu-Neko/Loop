using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class Entity : MonoBehaviour
{
    [SerializeField] private Transform playerCheck;

    public FiniteStateMachine StateMachine { get; private set; }
    [SerializeField] private D_Entity EntityData;

    public Core Core { get; private set; }
    private Movement movement;
    protected Stats stats;
    private Combat combat;

    public Animator Anim { get; private set; }
    private WeaponAttackDetails collisionAttackDetails;
    public bool SkillCollideDamage { get; private set; }

    public virtual void Awake()
    {
        Core = GetComponentInChildren<Core>();

        movement = Core.GetCoreComponent<Movement>();
        stats = Core.GetCoreComponent<Stats>();
        combat = Core.GetCoreComponent<Combat>();

        Anim = GetComponent<Animator>();
        collisionAttackDetails = EntityData.collisionAttackDetails;

        StateMachine = new();
    }
    public virtual void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        Anim.SetFloat("yVelocity", movement.RB.velocity.y);
    }
    public virtual void FixedUpdate()
    {
        StateMachine.CurrentState.DoChecks();
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.minAgroDistance, EntityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.maxAgroDistance, EntityData.whatIsPlayer);
    }
    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, EntityData.closeRangeActionDistance, EntityData.whatIsPlayer);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(transform.right * EntityData.maxAgroDistance), 0.2f);
    }
    private void AnimationActionTrigger()
    {
        StateMachine.CurrentState.AnimationActionTrigger();
    }
    private void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
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
                knockbackable.Knockback(knockBackAngle, knockBackForce, movement.FacingDirection, GetPosition(), blockable);
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
        if (collision.collider.CompareTag("Player"))
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
            if (collision.gameObject.TryGetComponent(out IKnockbackable knockbackable))
            {
                knockbackable.Knockback(collisionAttackDetails.knockbackAngle, collisionAttackDetails.knockbackForce, direction, GetPosition(), false);
            }
            if(collision.gameObject.TryGetComponent(out IDamageable damageable) && (EntityData.collideDamage || SkillCollideDamage))
            {
                damageable.Damage(collisionAttackDetails.damageAmount, GetPosition(), false);
            }
        }
    }

    public void SetSkillCollideDamage(bool value) => SkillCollideDamage = value;
}
