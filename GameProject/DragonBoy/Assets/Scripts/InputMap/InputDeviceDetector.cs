using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

namespace InputMap
{
    internal class InputDeviceDetector : MonoBehaviour
	{
		internal static InputDevice currentDevice;

		void OnEnable()
		{
			InputSystem.onActionChange += DetectCurrentInputDevice;
		}

		void OnDisable()
		{
			InputSystem.onActionChange -= DetectCurrentInputDevice;
		}

		void DetectCurrentInputDevice(object obj, InputActionChange change)
		{
			if (change == InputActionChange.ActionPerformed)
			{
                currentDevice = ((InputAction)obj).activeControl.device;
                if (currentDevice is Keyboard || currentDevice is Mouse)
                    ShowCursor();
                else
                    HideCursor();
            }
		}

        internal static void ShowCursor()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

        internal static void HideCursor()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;
		}

		internal static bool IsController() => currentDevice is Gamepad;
		internal static bool IsXboxController() => currentDevice is XInputController;
        internal static bool IsDualShockController() => currentDevice is DualShockGamepad;
        internal static bool IsSwitchController() =>
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            currentDevice is UnityEngine.InputSystem.Switch.SwitchProControllerHID;
#else
            false;
#endif
    }
}