using UnityEngine;
using UnityEngine.InputSystem;

namespace InputMap
{
    public class InputDeviceDetector : MonoBehaviour
	{
		static InputDeviceDetector instance;

		void Awake()
		{
			if (instance == null)
				instance = this;
		}

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
                InputDevice currentDevice = ((InputAction)obj).activeControl.device;
                if (currentDevice is Keyboard || currentDevice is Mouse)
                    ShowCursor();
                else
                    HideCursor();
            }
		}

		public static void ShowCursor()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		public static void HideCursor()
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Confined;
		}
	}
}