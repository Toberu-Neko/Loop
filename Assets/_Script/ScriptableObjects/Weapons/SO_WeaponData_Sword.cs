using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSwordData", menuName = "Data/WeaponData/Sword")]
public class SO_WeaponData_Sword : SO_WeaponData
{
    public int maxEnergy;

    public GameObject projectile;

    [SerializeField] private WeaponAttackDetails[] normalAttackDetails;

    [Header("Strong Attack")]
    public float strongAttackHoldTime;

    public float projectileDamage;
    public float projectileSpeed;
    public float projectileDuration;
    public float projectileKnockbackStrength;
    public Vector2 projectileKnockbackAngle;


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
