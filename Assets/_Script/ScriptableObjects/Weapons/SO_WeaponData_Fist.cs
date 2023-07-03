using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FistData", menuName = "Data/WeaponData/FistData")]
public class SO_WeaponData_Fist : SO_WeaponData
{
    [Tooltip("最大能量")]
    public int maxEnergy;

    [Header("Normal Attack")]
    public WeaponAttackDetails normalAttackDetails;

    [Header("Strong Attack")]
    public float strongAttackHoldTime;
    public float everySoulAddtionalHoldTime;
    public WeaponAttackDetails strongAttackDetails;
    [Space]
    public WeaponAttackDetails soulTwoAttackDetails;
    public WeaponAttackDetails soulThreeAttackDetails;
    public WeaponAttackDetails soulFourAttackDetails;
    public WeaponAttackDetails soulFiveAttackDetails;

    [Header("Skill Attack")]
    public WeaponAttackDetails skillAttackDetails;
}
