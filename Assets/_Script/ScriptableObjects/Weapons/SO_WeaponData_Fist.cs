using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FistData", menuName = "Data/WeaponData/FistData")]
public class SO_WeaponData_Fist : SO_WeaponData
{
    [Tooltip("最大能量")]
    public int maxEnergy;

    [Header("Normal Attack")]
    public float resetAttackTime;
    [SerializeField] private WeaponAttackDetails[] normalAttackDetails;

    [Header("Strong Attack")]
    public float strongAttackHoldTime;
    public float everySoulAddtionalHoldTime;
    public WeaponAttackDetails[] soulAttackDetails;

    [Header("Skill Attack")]
    public WeaponAttackDetails skillAttackDetails;

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
