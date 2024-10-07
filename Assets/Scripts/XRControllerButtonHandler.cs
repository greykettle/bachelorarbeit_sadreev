using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;


public class XRControllerButtonHandler : MonoBehaviour
{
    public UnityEngine.XR.Interaction.Toolkit.XRController controller; // Ссылка на XR Controller

    void Update()
    {
        // Проверяем состояние кнопки "A" на контроллере
        // (controller.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
        {
            Debug.Log("Button A pressed!");
            // Ваш код при нажатии кнопки "A"
        }
    }
}
