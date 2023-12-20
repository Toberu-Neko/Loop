using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSFX", menuName = "ScriptableObjects/PlayerSFX", order = 1)]
public class SO_PlayerSFX : ScriptableObject
{
    [Header("Movement")]
    public AudioClip footstep;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip dash;

    [Header("Attack")]
    public AudioClip[] swordAttack;
    public AudioClip swordHit;
    public AudioClip gunAttack;

    
}
