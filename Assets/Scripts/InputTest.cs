using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    [SerializeField] InputAction action;

    private void Update()
    {
        if (action.activeControl is ButtonControl buttonControl)
            Debug.Log(buttonControl.isPressed);
    }

  /*  protected virtual bool IsPressed)
    {
        if (action == null)
            return false;

#if INPUT_SYSTEM_1_1_OR_NEWER || INPUT_SYSTEM_1_1_PREVIEW // 1.1.0-preview.2 or newer, including pre-release
                return action.phase == InputActionPhase.Performed;
#else
       

        if (action.activeControl is AxisControl)
            return action.ReadValue<float>() >= m_ButtonPressPoint;

        return action.triggered || action.phase == InputActionPhase.Performed;
#endif
    }*/
}
