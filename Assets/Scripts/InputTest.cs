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
}
