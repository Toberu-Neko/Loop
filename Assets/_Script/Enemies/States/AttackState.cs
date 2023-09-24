using System.Linq;
using UnityEngine;

public class AttackState : EnemyState
{
    protected Transform attackPosition;

    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;

    public AttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition) : base(entity, stateMachine, animBoolName)
    {
        this.attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAgroRange = CheckPlayerSenses.IsPlayerInMinAgroRange;
        isPlayerInMaxAgroRange = CheckPlayerSenses.IsPlayerInMaxAgroRange;
    }

    public override void Enter()
    {
        base.Enter();

        isAnimationFinished = false;
        Movement.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Movement.SetVelocityX(0f);
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public void DoDamageToDamageList(float damageAmount, float damageStaminaAmount, Vector2 knockBackAngle, float knockBackForce, bool blockable = true)
    {
        if (Combat.DetectedDamageables.Count > 0)
        {
            foreach (IDamageable damageable in Combat.DetectedDamageables.ToList())
            {
                damageable.Damage(damageAmount, core.transform.position, blockable);
            }

            foreach (IKnockbackable knockbackable in Combat.DetectedKnockbackables.ToList())
            {
                knockbackable.Knockback(knockBackAngle, knockBackForce, Movement.FacingDirection, (Vector2)core.transform.position, blockable);
            }

            foreach (IStaminaDamageable staminaDamageable in Combat.DetectedStaminaDamageables.ToList())
            {
                staminaDamageable.TakeStaminaDamage(damageStaminaAmount, core.transform.position, blockable);
            }
            Combat.DetectedDamageables.Clear();

        }
    }
}
