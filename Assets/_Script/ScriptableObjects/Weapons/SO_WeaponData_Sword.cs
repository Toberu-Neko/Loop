using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSwordData", menuName = "Data/WeaponData/Sword")]
public class SO_WeaponData_Sword : SO_WeaponData
{
    [Tooltip("�̤j��q")]
    public int maxEnergy;

    [Tooltip("�W�O�����C��")]
    public GameObject projectile;

    [SerializeField] private WeaponAttackDetails[] normalAttackDetails;

    [Header("Sky Attack")]
    public WeaponAttackDetails skyAttackDetails;

    [Header("Strong Attack")]
    [Tooltip("�W�O�����ɶ�")]
    public float strongAttackHoldTime;

    public ProjectileDetails projectileDetails;

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
