using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseUI : MonoBehaviour
{
    private enum GameDevice
    {
        Keyboard,
        Gamepad
    }

    private GameDevice activeGameDevice;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private VirtualMouseInput virtualMouseInput;
    [SerializeField] private GameObject MouseObj;

    [SerializeField] private bool isUIOpen = false;

    private void Awake()
    {
        InputSystem.onActionChange += HandleActionChange;
    }

    private void Start()
    {
        UpdateCursor();
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= HandleActionChange;
    }

    private void HandleActionChange(object arg1, InputActionChange inputActionChange)
    {
        if(inputActionChange == InputActionChange.ActionPerformed && arg1 is InputAction)
        {
            InputAction inputAction = (InputAction)arg1;

            if (inputAction.activeControl.device.displayName == "VirtualMouse")
            {
                // Ignore the first device that is not a virtual mouse
                return;
            }

            Debug.Log("Change detected.");

            if (inputAction.activeControl.device is Gamepad)
            {
                if (activeGameDevice != GameDevice.Gamepad)
                {
                    Debug.Log(inputAction.activeControl.device.displayName);
                    ChangeActiveGameDevice(GameDevice.Gamepad);
                }
            }
            else if((inputAction.activeControl.device is Keyboard && inputAction.activeControl.device is not Gamepad) || inputAction.activeControl.device is Mouse)
            {
                if (activeGameDevice != GameDevice.Keyboard)
                {
                    Debug.Log("Keyboard: " + (inputAction.activeControl.device is Keyboard) + " Mouse: " + (inputAction.activeControl.device is Mouse));
                    Debug.Log(inputAction.activeControl.device.displayName);
                    ChangeActiveGameDevice(GameDevice.Keyboard);
                }
            }
        }
    }

    private void ChangeActiveGameDevice(GameDevice newGameDevice)
    {
        activeGameDevice = newGameDevice;
        Cursor.visible = activeGameDevice == GameDevice.Keyboard;

        UpdateCursor();
    }

    public void SetIsUIOpen(bool isUIOpen)
    {
        this.isUIOpen = isUIOpen;
        UpdateCursor();
    }

    public void UpdateCursor()
    {
        if (activeGameDevice == GameDevice.Gamepad && isUIOpen)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        MouseObj.SetActive(true);
    }

    private void Hide()
    {
        MouseObj.SetActive(false);
    }

    private void Update()
    {
        transform.localScale = Vector3.one * canvasRectTransform.localScale.x;
        transform.SetAsLastSibling();
    }

    private void LateUpdate()
    {
        Vector2 virtualMousePosition = virtualMouseInput.virtualMouse.position.value;
        virtualMousePosition.x = Mathf.Clamp(virtualMousePosition.x, 0, Screen.width);
        virtualMousePosition.y = Mathf.Clamp(virtualMousePosition.y, 0, Screen.height);
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePosition);
    }
}
