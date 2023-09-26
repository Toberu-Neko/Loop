using UnityEngine;

public class PlayerGunChargingState : PlayerGunAttackState
{
    private bool holdAttackInput;
    private SO_WeaponData_Gun data;
    private GunChargeAttackScript chargeAttack;
    private bool shootable;
    private bool moveable;
    private bool shot;

    private float chargeTime;
    private float lastDamageTime;
    private int xInput;

    public PlayerGunChargingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        data = player.PlayerWeaponManager.GunData;
        chargeAttack = player.PlayerWeaponManager.GunChargeAttackScript;
    }

    public override void Enter()
    {
        base.Enter();
        Combat.OnDamaged += () => isAttackDone = true;

        player.PlayerWeaponManager.SetGunRegenable(false);
        player.PlayerWeaponManager.DecreaseGunEnergy(data.chargeAttackEnergyCostPerSecond);

        lastDamageTime = 0;
        shootable = true;
        moveable = true;
        shot = false;
    }

    public override void Exit()
    {
        base.Exit();
        Combat.OnDamaged -= () => isAttackDone = true;

        player.Anim.SetBool("gunChargeShoot", false);
        chargeAttack.gameObject.SetActive(false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        xInput = player.InputHandler.NormInputX;
        holdAttackInput = player.InputHandler.HoldAttackInput;

        if (moveable)
        {
            Movement.CheckIfShouldFlip(xInput);
            Movement.SetVelocityX(playerData.movementVelocity * data.chargeMovementSpeedMultiplier * xInput);
        }

        if (holdAttackInput && player.PlayerWeaponManager.GunCurrentEnergy > 0)
        {
            player.PlayerWeaponManager.DecreaseGunEnergy(data.chargeAttackEnergyCostPerSecond * Time.deltaTime);
        }
        else if(shootable)
        {
            player.Anim.SetBool("gunChargeShoot", true);
            shootable = false;
            moveable = false;
        }

        if (shot && Time.time >= lastDamageTime + data.chargeAttackDamagePace)
        {
            // Debug.Log("Shoot at " + Time.time);
            lastDamageTime = Time.time;
            DoDamageToDamageList(
                WeaponType.Gun,
                data.chargeAttackDamageAmount * chargeTime,
                data.chargeAttackStaminaDamageAmount,
                data.chargeAttackKnockbackAngle,
                data.chargeAttackKnockbackForce);
        }
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();

        Movement.SetVelocityX(data.chargeAttackBackFireVelocity * -Movement.FacingDirection * chargeTime);
    }
    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();

        Movement.SetVelocityX(0f);
        chargeAttack.gameObject.SetActive(false);
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        chargeTime = Time.time - StartTime;
        chargeAttack.Init(new Vector2(chargeTime * data.chargeAttackWidthPerSecond, data.chargeAttackHeight));
        chargeAttack.gameObject.SetActive(true);

        player.PlayerWeaponManager.GunFiredRegenDelay();
        shot = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAttackDone = true;
    }
}
