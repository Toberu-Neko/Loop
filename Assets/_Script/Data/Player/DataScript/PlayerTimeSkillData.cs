using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTimeSkillData", menuName = "Data/Player Data/Time Data")]
public class PlayerTimeSkillData : ScriptableObject
{
    public float maxEnergy = 100f;

    [Header("Rewind Player")]
    [Tooltip("�Ω�M����m��T")]
    public float rewindMaxTime = 10f;
    [Tooltip("�C����ӯ�q")]
    public float rewindCostPerSecond = 10f;
}
