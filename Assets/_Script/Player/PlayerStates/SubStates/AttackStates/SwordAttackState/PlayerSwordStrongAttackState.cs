using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordStrongAttackState : PlayerSwordAttackState
{
    private SO_WeaponData_Sword weaponData;
    private bool startMovement;
    public PlayerSwordStrongAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        weaponData = player.PlayerWeaponManager.SwordData;
    }
    public override void Enter()
    {
        base.Enter();

        Combat.OnDamaged += HandleOnDamaged;
        startMovement = false;
    }

    public override void Exit()
    {
        base.Exit();

        Combat.OnDamaged -= HandleOnDamaged;
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

        if(player.PlayerWeaponManager.SwordCurrentEnergy >= weaponData.strongAttackEnergyCost)
        {
            player.PlayerWeaponManager.DecreaseEnergy();
            CamManager.Instance.CameraShake();
            GameObject projectile = ObjectPoolManager.SpawnObject(player.PlayerWeaponManager.SwordData.projectile, core.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.Projectiles);
            PlayerProjectile projectileScript = projectile.GetComponent<PlayerProjectile>();
            ProjectileDetails details = weaponData.projectileDetails;
            details.damageAmount *= PlayerInventoryManager.Instance.SwordMultiplier.attackSpeedMultiplier;
            projectileScript.Fire(weaponData.projectileDetails, new Vector2(Movement.FacingDirection, 0));
        }
        else
        {
            DoDamageToDamageList(WeaponType.Sword, weaponData.strongAttackDetails.damageAmount, weaponData.strongAttackDetails.staminaDamageAmount, weaponData.strongAttackDetails.knockbackAngle, weaponData.strongAttackDetails.knockbackForce);
        }
    }

    private void HandleOnDamaged()
    {
        isAttackDone = true;
    }
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAttackDone = true;
    }

}
