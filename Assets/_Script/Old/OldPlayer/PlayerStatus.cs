using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool jumpAndDashAble;
    public bool moveable;
    public float movementMultiplier;
    private void Awake()
    {
        moveable = true;
        jumpAndDashAble = true;
        movementMultiplier = 1f;
    }
}
