using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FistData", menuName = "Data/WeaponData/FistData")]
public class SO_WeaponData_Fist : SO_WeaponData
{
    [Tooltip("最大能量")]
    public int maxEnergy;

    [Tooltip("重制武器攻擊計數器所需的非攻擊時間")]
    [Header("Normal Attack")]
    public float resetAttackTime;
    [SerializeField] private WeaponAttackDetails[] normalAttackDetails;

    [Header("Strong Attack")]
    [Tooltip("觸發第一次蓄力攻擊所需的時間")]
    public float strongAttackHoldTime;
    [Tooltip("每次充能鬥魂所需的時間")]
    public float everySoulAddtionalHoldTime;
    public WeaponAttackDetails[] soulAttackDetails;

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
