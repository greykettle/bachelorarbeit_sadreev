using System.Collections;
using UnityEngine;

public class DelayedPositionSetter : MonoBehaviour
{
    // Локальные координаты, на которые нужно переместить объект
    public Vector3 targetLocalPosition = new Vector3(1, 1, 1);
    public Vector3 targetLocalRotation = new Vector3(0, 0, 0);

    // Время задержки в секундах
    public float delayTime = 10.0f;

    void Start()
    {
        // Запуск корутины для установки позиции с задержкой
        StartCoroutine(SetPositionWithDelay());
    }

    IEnumerator SetPositionWithDelay()
    {
        // Ожидание заданное количество секунд
        yield return new WaitForSeconds(delayTime);

        // Установка локальной позиции и поворота объекта
        transform.localPosition = targetLocalPosition;
        transform.localRotation = Quaternion.Euler(targetLocalRotation);

        // Отключение движения
        FreezeObject();

        // Логгирование для проверки
        Debug.Log($"Object local position set to: {transform.localPosition}, local rotation set to: {transform.localRotation.eulerAngles}. Object movement is now disabled.");
    }

    void FreezeObject()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Отключение гравитации и замораживание движения
            rb.useGravity = false;
            rb.velocity = Vector3.zero; // Остановка движения
            rb.angularVelocity = Vector3.zero; // Остановка вращения
            rb.isKinematic = true; // Кинематический режим для фиксации объекта
        }
    }
}
