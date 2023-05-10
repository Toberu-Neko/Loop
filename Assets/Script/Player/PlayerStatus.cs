using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    public bool jumpAndDashAble;
    public bool moveable;
    public float movementMultiplier;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        moveable = true;
        jumpAndDashAble = true;
        movementMultiplier = 1f;
    }
    
    private void Start()
    {
        
    }
}
