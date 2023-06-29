using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Data/WeaponData/Gun Data")]
public class SO_WeaponData_Gun : SO_WeaponData
{
    public float maxEnergy;
    public float energyRegen;
    public float energyRegenDelay;

    [Header("NormalAttack")]
    public GameObject normalAttackObject;
    public float energyCostPerShot;
    public float attackSpeed;
    public ProjectileDetails normalAttackDetails;

    [Header("ChargeAttack")]
    public float chargeMovementSpeedMultiplier = 0.3f;
    public float chargeAttackBackFireVelocity = 3f;
    public float minChargeTime;
    public float chargeAttackHeight = 1f;
    public float chargeAttackWidthPerSecond = 0.5f;
    public float chargeAttackEnergyCostPerSecond;

    public float chargeAttackDamagePace = 0.2f;
    public float chargeAttackDamageAmount;
    public float chargeAttackStaminaDamageAmount;
    public float chargeAttackKnockbackForce;
    public Vector2 chargeAttackKnockbackAngle;
}
