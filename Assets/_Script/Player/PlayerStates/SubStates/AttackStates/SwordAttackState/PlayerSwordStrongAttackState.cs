using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordStrongAttackState : PlayerSwordAttackState
{
    private SO_WeaponData_Sword weaponData;
    private bool startMovement;
    // private bool fireObj;
    public PlayerSwordStrongAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        weaponData = player.WeaponManager.SwordData;
    }
    public override void Enter()
    {
        base.Enter();

        Combat.OnKnockback += HandleOnKnockback;
        startMovement = false;
        /*
        fireObj = false;

        if (player.WeaponManager.SwordCurrentEnergy >= weaponData.strongAttackEnergyCost)
        {
            fireObj = true;
        }
        */
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnKnockback -= HandleOnKnockback;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (startMovement)
        {
            Movement.SetVelocityX(weaponData.strongAttackDetails.movementSpeed * Movement.FacingDirection);
        }
    }

    public override void AnimationStartMovementTrigger()
    {
        base.AnimationStartMovementTrigger();
        startMovement = true;
    }

    public override void AnimationStopMovementTrigger()
    {
        base.AnimationStopMovementTrigger();
        startMovement = false;
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();
        /*
        if(fireObj)
        {
            player.WeaponManager.DecreaseEnergy();
            CamManager.Instance.CameraShake();
            GameObject projectile = ObjectPoolManager.SpawnObject(player.WeaponManager.SwordData.projectile, core.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
            PlayerProjectile projectileScript = projectile.GetComponent<PlayerProjectile>();
            ProjectileDetails details = weaponData.projectileDetails;
            details.combatDetails.damageAmount *= PlayerInventoryManager.Instance.SwordMultiplier.attackSpeedMultiplier;
            projectileScript.Fire(weaponData.projectileDetails, new Vector2(Movement.FacingDirection, 0));
        }
        else
        {
        }*/
        DoDamageToDamageList(WeaponType.Sword, weaponData.strongAttackDetails);
    }

    private void HandleOnKnockback()
    {
        isAttackDone = true;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }

}
