using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSwordData", menuName = "Data/WeaponData/Sword")]
public class SO_WeaponData_Sword : SO_WeaponData
{
    [Tooltip("最大能量")]
    public int maxEnergy;

    [Header("Normal Attack")]
    public float resetAttackTime;
    [SerializeField] private WeaponAttackDetails[] normalAttackDetails;

    [Header("Sky Attack")]
    public WeaponAttackDetails skyAttackDetails;

    [Header("Strong Attack")]
    public WeaponAttackDetails strongAttackDetails;
    [Tooltip("蓄力攻擊時間")]
    public float strongAttackHoldTime;
    public int strongAttackEnergyCost;
    public GameObject projectile;
    public ProjectileDetails projectileDetails;


    [Header("Counter Attack")]
    public WeaponAttackDetails counterAttackDetails;

    [Header("Soul One Attack")]
    public WeaponAttackDetails soulOneAttackDetails;

    [Header("Soul Three Attack")]
    public WeaponAttackDetails soulThreeAttackDetails;

    public WeaponAttackDetails[] NormalAttackDetails { get => normalAttackDetails; private set => normalAttackDetails = value; }
    private void OnEnable()
    {
        AmountOfAttacks = normalAttackDetails.Length;

        MovementSpeed = new float[AmountOfAttacks];

        for (int i = 0; i < AmountOfAttacks; i++)
        {
            MovementSpeed[i] = normalAttackDetails[i].movementSpeed;
        }
    }
}
