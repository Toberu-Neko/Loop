using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerTimeSkillData", menuName = "Data/Player Data/Time Data")]
public class PlayerTimeSkillData : ScriptableObject
{
    public float maxEnergy = 100f;

    public float rewindCostPerSecond = 5f;
    public float rewindExtraCostPerSecond = 2f;
}
