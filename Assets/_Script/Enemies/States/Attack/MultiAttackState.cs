using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAttackState : AttackState
{
    protected ED_MultiAttackState stateData;

    private bool startAttack;

    private int count = 0;
    public MultiAttackState(Entity entity, EnemyStateMachine stateMachine, string animBoolName, Transform attackPosition, ED_MultiAttackState stateData) : base(entity, stateMachine, animBoolName, attackPosition)
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();

        if(stateData.thingsToSay != null)
        {
            if (stateData.thingsToSay.Length > 0 && !saidThings)
            {
                foreach (var thing in stateData.thingsToSay)
                {
                    UI_Manager.Instance.ActivateTutorialPopUpUI(thing);
                }

                saidThings = true;
            }
        }

        count = 0;
        startAttack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Stats.IsAngry && startAttack)
        {
            if (CheckPlayerSenses.IsPlayerInMaxAgroRange)
            {
                Movement.SetVelocityX(stateData.angryMoveSpeed * Movement.FacingDirection);
            }
            else
            {
                Movement.SetVelocityX(stateData.angryMoveSpeed* -Movement.FacingDirection);
            }
        }

        entity.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        startAttack = true;

        if (count >= stateData.details.Length)
        {
            count++;
            return;
        }

        WeaponAttackDetails details = stateData.details[count];

        DoDamageToDamageList(details.damageAmount, details.staminaDamageAmount, details.knockbackAngle, details.knockbackForce, false);

        count++;
    }

    public bool CheckCanAttack()
    {
        return Time.time >= EndTime + stateData.attackCooldown;
    }

}
