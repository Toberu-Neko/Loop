using UnityEngine;
using UnityEngine.InputSystem;

public class SetVidText : MonoBehaviour
{
    [SerializeField] private GameObject keyboardText;
    [SerializeField] private GameObject gamepadText;

    private enum GameDevice
    {
        Keyboard,
        Gamepad
    }

    private GameDevice activeGameDevice;
    private void Awake()
    {
        InputSystem.onActionChange += HandleActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= HandleActionChange;
    }
    private void HandleActionChange(object arg1, InputActionChange inputActionChange)
    {
        if (inputActionChange == InputActionChange.ActionPerformed && arg1 is InputAction)
        {
            InputAction inputAction = (InputAction)arg1;

            if (inputAction.activeControl.device.displayName == "VirtualMouse")
            {
                // Ignore the first device that is not a virtual mouse
                return;
            }

            if (inputAction.activeControl.device is Gamepad)
            {
                if (activeGameDevice != GameDevice.Gamepad)
                {
                    ChangeActiveGameDevice(GameDevice.Gamepad);
                    gamepadText.SetActive(true);
                    keyboardText.SetActive(false);
                }
            }
            else if ((inputAction.activeControl.device is Keyboard && inputAction.activeControl.device is not Gamepad) || inputAction.activeControl.device is Mouse)
            {
                if (activeGameDevice != GameDevice.Keyboard)
                {
                    ChangeActiveGameDevice(GameDevice.Keyboard);
                    gamepadText.SetActive(false);
                    keyboardText.SetActive(true);

                }
            }
        }
    }
    private void ChangeActiveGameDevice(GameDevice newGameDevice)
    {
        activeGameDevice = newGameDevice;
    }
}
