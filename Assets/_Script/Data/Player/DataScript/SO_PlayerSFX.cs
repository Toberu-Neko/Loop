using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSFX", menuName = "ScriptableObjects/PlayerSFX", order = 1)]
public class SO_PlayerSFX : ScriptableObject
{
    [Header("Movement")]
    public Sound footstep;
    public Sound jump;
    public Sound land;

    [Header("Skill")]
    public Sound turnOn;
    public Sound heal;
    public Sound dash;

    [Header("Attack")]
    public Sound[] swordAttack;
    public Sound swordHit;
    public Sound gunAttack;
    public Sound getHit;
    public Sound perfectBlock;

    
}
