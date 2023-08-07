using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTimeSkillData", menuName = "Data/Player Data/Time Data")]
public class PlayerTimeSkillData : ScriptableObject
{
    public float maxEnergy = 100f;

    [Header("Rewind Player")]
    [Range(1,5)]
    public int rewindPlaySpeed = 1;
    [Tooltip("�Ω�M����m��T")]
    public float rewindMaxTime = 10f;
    [Tooltip("�C����ӯ�q")]
    public float rewindCostPerSecond = 10f;

    [Header("Book Mark")]
    [Tooltip("�C����ӯ�q")]
    public float bookMarkCostPerSecond = 10f;
    [Range(1, 5)]
    public int bookmarkPlaySpeed = 1;
    public GameObject bookMarkPrefab;

    [Header("Time Stop All")]
    [Tooltip("�C����ӯ�q")]
    public float timeStopAllCostPerSecond = 20f;

    [Header("Bullet Time All")]
    public float bulletTimeAllDuration = 3f;
    public float bulletTimeAllCost = 50f;
    [Range(0.001f, 1f)]
    public float bulletTimeVelocityMultiplier = 0.2f;

    [Header("Time Stop Throw")]
    [Tooltip("�C����ӯ�q")]
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
