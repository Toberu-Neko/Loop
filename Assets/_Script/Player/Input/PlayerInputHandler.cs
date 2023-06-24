using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera cam;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }

    public bool JumInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool AttackInput { get; private set; }
    public bool HoldAttackInput { get; private set; }
    public bool BlockInput { get; private set; }

    public bool WeaponSkillInput { get; private set; }
    public bool WeaponSkillHoldInput { get; private set; }
    public bool ChangeWeapon1 { get; private set; }
    public bool ChangeWeapon2 { get; private set; }
    public bool ChangeWeapon3 { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Debug.Log("AttackInput");
            AttackInput = true;
            HoldAttackInput = true;
        }
        if (context.canceled)
        {
            AttackInput = false;
            HoldAttackInput = false;
        }
    }
    public void UseAttackInput() => AttackInput = false;

    public void OnBlockInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BlockInput = true;
        }
        if (context.canceled)
        {
            BlockInput = false;
        }
    }

    public void OnWeaponSkillInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            WeaponSkillInput = true;
            WeaponSkillHoldInput = true;
        }
        if (context.canceled)
        {
            WeaponSkillInput = false;
            WeaponSkillHoldInput = false;
        }
    }

    public void OnChangeWeapon1Input(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            ChangeWeapon1 = true;
        }
        if(context.canceled)
        {
            ChangeWeapon1 = false;
        }
    }

    public void OnChangeWeapon2Input(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ChangeWeapon2 = true;
        }
        if (context.canceled)
        {
            ChangeWeapon2 = false;
        }
    }

    public void OnChangeWeapon3Input(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ChangeWeapon3 = true;
        }
        if (context.canceled)
        {
            ChangeWeapon3 = false;
        }
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = UnityEngine.Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = UnityEngine.Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void UseDashInput() => DashInput = false;

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        if(playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
            RawDashDirectionInput.Normalize();
        }
        //45 degree angle
        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            JumpInput = true;
            JumInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if(context.canceled)
        {
            JumInputStop = true;
        }
    }
    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            GrabInput = true;
        }
        if(context.canceled)
        {
            GrabInput = false;
        }
    }
    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }
}

public enum CombatInputs
{
    primary,
    secondary
}