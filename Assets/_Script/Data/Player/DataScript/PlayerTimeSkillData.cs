using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTimeSkillData", menuName = "Data/Player Data/Time Data")]
public class PlayerTimeSkillData : ScriptableObject
{
    public float maxEnergy = 100f;

    [Header("Rewind Player")]
    [Tooltip("用於清除位置資訊")]
    public float rewindMaxTime = 10f;
    [Tooltip("每秒消耗能量")]
    public float rewindCostPerSecond = 10f;
}
