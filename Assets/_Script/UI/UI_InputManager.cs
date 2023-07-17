using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI_InputManager : MonoBehaviour
{
    public bool ESCInput { get; private set; } 

    public void OnESCInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ESCInput = true;
        }
        if (context.canceled)
        {
            ESCInput = false;
        }
    }

    public void UseESCInput() => ESCInput = false;
}
