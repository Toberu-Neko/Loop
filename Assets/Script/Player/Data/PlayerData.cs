using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("MoveState")]
    public float movementVelocity = 10f;

    [Header("JumpState")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("InAirState")]
    public float coyoteTime = 0.2f;

    [Header("CheckVariables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
