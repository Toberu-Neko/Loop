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
    public ProjectileDetails normalAttackDetails;
}
