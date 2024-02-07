using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "PlayerTimeSkillData", menuName = "Data/Player Data/Time Data")]
public class PlayerTimeSkillData : ScriptableObject
{
    public float maxEnergy = 100f;
    public LayerMask whatIsInteractable;
    public float attackInvreaseEnergy = 5f;
    public float perfectBolckIncreaseEnergy = 20f;

    [Header("None")]
    public LocalizedString noneSkillName;

    [Header("Rewind Player")]
    public LocalizedString rewindSkillName;
    [Range(1, 5)]
    public int rewindPlaySpeed = 1;
    [Tooltip("用於清除位置資訊")]
    public float rewindMaxTime = 10f;
    [Tooltip("每秒消耗能量")]
    public float rewindCostPerSecond = 10f;

    [Header("Book Mark")]
    public LocalizedString bookMarkSkillName;
    [Tooltip("每秒消耗能量")]
    public float bookMarkCostPerSecond = 10f;
    [Range(1, 5)]
    public int bookmarkPlaySpeed = 1;
    public GameObject bookMarkPrefab;

    [Header("Time Stop All")]
    public LocalizedString timeStopAllSkillName;
    [Tooltip("每秒消耗能量")]
    public float timeStopAllCostPerSecond = 20f;

    [Header("Bullet Time All")]
    public LocalizedString bulletTimeAllSkillName;
    public float bulletTimeAllDuration = 3f;
    public float bulletTimeAllCost = 50f;

    [Header("Bullet Time Ranged")]
    public LocalizedString bulletTimeRangedSkillName;
    public float bulletTimeRangedDuration = 3f;
    public float bulletTimeRangedCost = 50f;
    public float bulletTimeRangedRadius = 3f;

    [Header("Time Stop Throw")]
    public LocalizedString timeStopThrowSkillName;
    [Tooltip("每秒消耗能量")]
    public float timeStopThrowCost = 15f;
    [Header("Throw Object")]
    public GameObject timeStopThrowObj;
    public float throwVelocityIncreaseRate = 2f;
    public float throwStopTime = 3f;
    public float minThrowVelocity = 5f;
    public float maxThrowVelocity = 20f;
    public float gravityScale = 5f;

    [Header("Predict Line")]
    public GameObject predictLineObj;
    public int numberOfPredictLineObj = 25;
    public float spaceBetweenPredictLineObj = 0.2f;
}
