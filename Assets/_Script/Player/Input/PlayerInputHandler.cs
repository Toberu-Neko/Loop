using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private GameManager gameManager;
    private Camera cam;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawMouseDirectionInput { get; private set; }
    public Vector2Int FixedMouseDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }

    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    #region Combat Inputs
    public bool AttackInput { get; private set; }
    public bool HoldAttackInput { get; private set; }
    public bool BlockInput { get; private set; }

    public bool WeaponSkillInput { get; private set; }
    public bool WeaponSkillHoldInput { get; private set; }
    public bool ChangeWeapon1 { get; private set; }
    public bool ChangeWeapon2 { get; private set; }
    public bool ChangeWeapon3 { get; private set; }
    #endregion

    public bool TimeSkillInput { get; private set; }
    public bool TimeSkillHoldInput { get; private set; }

    public bool DebugInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool ESCInput { get; private set; }

    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        gameManager = GameManager.Instance;

        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void ResetAllInput()
    {
        RawMovementInput = Vector2.zero;
        NormInputX = 0;
        NormInputY = 0;
        JumpInput = false;
        JumpInputStop = false;
        GrabInput = false;
        DashInput = false;
        DashInputStop = false;
        AttackInput = false;
        HoldAttackInput = false;
        BlockInput = false;
        WeaponSkillInput = false;
        WeaponSkillHoldInput = false;
        ChangeWeapon1 = false;
        ChangeWeapon2 = false;
        ChangeWeapon3 = false;
        TimeSkillInput = false;
        TimeSkillHoldInput = false;
        DebugInput = false;
    }

    #region ESC
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

    #endregion

    public void OnDebugInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if(context.started)
        {
            DebugInput = true;
        }
        if(context.canceled)
        {
            DebugInput = false;
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if(context.started)
        {
            InteractInput = true;
        }
        if(context.canceled)
        {
            InteractInput = false;
        }
    }

    public void UseInteractInput()
    {
        InteractInput = false;
    }

    public void OnTimeSkillInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            TimeSkillInput = true;
            TimeSkillHoldInput = true;
        }
        if (context.canceled)
        {
            TimeSkillInput = false;
            TimeSkillHoldInput = false;
        }
    }
    public void UseTimeSkillInput() => TimeSkillInput = false;

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

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
        if (gameManager.IsPaused)
            return;

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
        if (gameManager.IsPaused)
            return;

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
    public void UseWeaponSkillInput() => WeaponSkillInput = false;

    public void OnChangeWeapon1Input(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
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
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            ChangeWeapon2 = true;
        }
        if (context.canceled)
        {
            ChangeWeapon2 = false;
        }
    }

    public void UseChangeWeapon2()
    {
        ChangeWeapon2 = false;
    }

    public void OnChangeWeapon3Input(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

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
        if (gameManager.IsPaused)
            return;

        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = UnityEngine.Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = UnityEngine.Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

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

    public void OnMouseDirectionInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        RawMouseDirectionInput = context.ReadValue<Vector2>();

        if(playerInput.currentControlScheme == "Keyboard" && cam!=null)
        {
            // Debug.Log(((Vector3)RawMouseDirectionInput - cam.WorldToScreenPoint(transform.position)).normalized);
            // Debug.Log(cam.ScreenToWorldPoint((Vector3)RawMouseDirectionInput) - transform.position);
            // RawMouseDirectionInput = cam.ScreenToWorldPoint((Vector3)RawMouseDirectionInput) - transform.position;
            RawMouseDirectionInput = ((Vector3)RawMouseDirectionInput - cam.WorldToScreenPoint(transform.position)).normalized;
            RawMouseDirectionInput = RawMouseDirectionInput.normalized;
        }

        //45 degree angle
        FixedMouseDirectionInput = Vector2Int.RoundToInt(RawMouseDirectionInput.normalized);

    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if(context.canceled)
        {
            JumpInputStop = true;
        }
    }
    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (gameManager.IsPaused)
            return;

        if (context.started)
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