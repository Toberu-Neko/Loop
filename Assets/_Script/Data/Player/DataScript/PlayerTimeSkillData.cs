using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTimeSkillData", menuName = "Data/Player Data/Time Data")]
public class PlayerTimeSkillData : ScriptableObject
{
    public float maxEnergy = 100f;
    [Range(1,5)]
    public int rewindPlaySpeed = 1;
    [Header("Rewind Player")]
    [Tooltip("用於清除位置資訊")]
    public float rewindMaxTime = 10f;
    [Tooltip("每秒消耗能量")]
    public float rewindCostPerSecond = 10f;

    [Header("Book Mark")]
    [Tooltip("每秒消耗能量")]
    public float bookMarkCostPerSecond = 10f;
    public GameObject bookMarkPrefab;
}
