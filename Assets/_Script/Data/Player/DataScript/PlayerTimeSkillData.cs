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
    [Tooltip("�Ω�M����m��T")]
    public float rewindMaxTime = 10f;
    [Tooltip("�C����ӯ�q")]
    public float rewindCostPerSecond = 10f;

    [Header("Book Mark")]
    [Tooltip("�C����ӯ�q")]
    public float bookMarkCostPerSecond = 10f;
    public GameObject bookMarkPrefab;

    [Header("Time Stop All")]
    [Tooltip("�C����ӯ�q")]
    public float timeStopAllCostPerSecond = 20f;

    public float throwVelocityIncreaseRate = 2f;
    public float throwStopTime = 3f;
    public float minThrowVelocity = 5f;
    public float maxThrowVelocity = 20f;
    public GameObject timeStopThrowObj;
    public GameObject predictLineObj;
}
