using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected SO_WeaponData weaponData;

    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected OldPlayerAttackState state;
    
    protected Core core;
    protected Movement Movement => movement ? movement : core.GetCoreComponent<Movement>();
    private Movement movement;

    protected CollisionSenses CollisionSenses => collisionSenses ? collisionSenses : core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    protected int attackCounter;

    protected float startTime;

    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        gameObject.SetActive(false);

    }
    protected virtual void Update()
    {
        
    }

    public virtual void EnterWeapon()
    {
        startTime = Time.time;

        gameObject.SetActive(true);

        if (attackCounter >= weaponData.AmountOfAttacks)
        {
            attackCounter = 0;
        }

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }
    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("attack", false);
        weaponAnimator.SetBool("attack", false);

        attackCounter++;

        gameObject.SetActive(false);
    }
    #region Animation Triggers
    public virtual void AnimationStartMovementTrigger()
    {
        if(CollisionSenses.Ground)
            state.SetPlayerVelocity(weaponData.MovementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        if (CollisionSenses.Ground)
            state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationActionTrigger()
    {
        // state.AnimationActionTrigger();
    }

    #endregion

    public void InitializeWeapon(OldPlayerAttackState state, Core core)
    {
        this.state = state;
        this.core = core;
    }
}
