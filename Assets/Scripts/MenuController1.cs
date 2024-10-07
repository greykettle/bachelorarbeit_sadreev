using UnityEngine;
using HTC.UnityPlugin.Vive;
using UnityEngine.InputSystem;

public class MenuController1 : MonoBehaviour
{
    private XRIDefaultInputActions inputActions;
    public GameObject menuCanvas;
    void Awake()
    {
        inputActions = new XRIDefaultInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.XRIRightHand.ButtonAPressed.performed += OnButtonAPressed;
    }

    void OnDisable()
    {
        inputActions.XRIRightHand.ButtonAPressed.performed -= OnButtonAPressed;
        inputActions.Disable();
    }
    private void OnButtonAPressed(InputAction.CallbackContext context)
    {
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        print("Toggle Menu");
        menuCanvas.SetActive(!menuCanvas.activeSelf);

        if (menuCanvas.activeSelf)
        {
            Transform cameraTransform = Camera.main.transform;
            menuCanvas.transform.position = cameraTransform.position + cameraTransform.forward * 2f;
            menuCanvas.transform.LookAt(cameraTransform);
            menuCanvas.transform.Rotate(0, 180, 0);
        }
    }
}
