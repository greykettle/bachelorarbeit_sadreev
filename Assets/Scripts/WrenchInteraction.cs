using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WrenchInteraction : MonoBehaviour
{
    public Transform previousDetail; // Предыдущая деталь, к которой фиксируется текущая
    public Transform testParent; // Родительский объект для зафиксированной детали
    public float snapDistance = 0.1f; // Допустимое расстояние для фиксации
    public float customYPosition = 0.0f; // Заданная координата Y
    public GameObject targetObject; // Целевой объект для триггера
    public float rotationDuration = 1.0f; // Время для анимации перемещения и вращения

    private bool isSnapped = false; // Флаг для отслеживания состояния фиксации
    private Rigidbody currentDetailRigidbody;
    private MeshCollider currentDetailMeshCollider;

    private Transform currentDetail;
    private Transform assembledParent;
    private GearboxAssembly gearboxAssembly;

    public GearboxAssembly GearboxAssembly
    {
        get { return gearboxAssembly; }
        set { gearboxAssembly = value; }
    }

    void Start()
    {
        currentDetail = this.transform;
        assembledParent = GameObject.Find("Assembled").transform;
        currentDetailRigidbody = currentDetail.GetComponent<Rigidbody>();
        currentDetailMeshCollider = this.GetComponent<MeshCollider>();

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        if (gearboxAssembly == null)
        {
            Debug.LogError("GearboxAssembly не найден в сцене. Убедитесь, что он присутствует и активен.");
        }
    }

    void FixedUpdate()
    {
        if (!isSnapped && currentDetail != null && previousDetail != null)
        {
            float distance = Vector3.Distance(previousDetail.position, currentDetail.position);

            if (distance < snapDistance)
            {
                SnapToPositionWithYOffset();
            }
        }
    }

    void SnapToPositionWithYOffset()
    {
        Transform assembledPreviousDetail = assembledParent.Find(previousDetail.name);
        Transform assembledCurrentDetail = assembledParent.Find(currentDetail.name);

        if (assembledPreviousDetail == null || assembledCurrentDetail == null)
        {
            Debug.LogError("Не удалось найти соответствующую деталь в assembled.");
            return;
        }

        // Вычисляем смещение позиции относительно предыдущей детали
        Vector3 positionOffset = assembledCurrentDetail.position - assembledPreviousDetail.position;

        // Задаем конкретную координату Y
        positionOffset.y = customYPosition;

        // Вычисляем смещение вращения
        Quaternion rotationOffset = assembledCurrentDetail.rotation * Quaternion.Inverse(assembledPreviousDetail.rotation);

        // Фиксируем текущую деталь в новой позиции и с новым вращением
        currentDetail.position = previousDetail.position + positionOffset;
        currentDetail.rotation = rotationOffset * previousDetail.rotation;

        // Отключаем возможность дальнейшего взаимодействия с деталью
        currentDetail.GetComponent<XRGrabInteractable>().enabled = false;

        // Отключаем физику, чтобы деталь не двигалась
        if (currentDetailRigidbody != null)
        {
            currentDetailRigidbody.useGravity = false;
            currentDetailRigidbody.isKinematic = true;
        }

        isSnapped = true;
        currentDetail.parent = testParent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject)
        {
            Debug.Log("Collied");
            StartCoroutine(SnapToFinalPosition());
        }
    }

    IEnumerator SnapToFinalPosition()
    {
        Transform assembledPreviousDetail = assembledParent.Find(previousDetail.name);
        Transform assembledCurrentDetail = assembledParent.Find(currentDetail.name);

        if (assembledPreviousDetail == null || assembledCurrentDetail == null)
        {
            Debug.LogError("Не удалось найти соответствующую деталь в assembled.");
            yield break;
        }

        // Вычисляем целевую позицию и вращение
        Vector3 targetPosition = previousDetail.position + (assembledCurrentDetail.position - assembledPreviousDetail.position);
        Quaternion targetRotation = assembledCurrentDetail.rotation * Quaternion.Inverse(assembledPreviousDetail.rotation) * previousDetail.rotation;

        // Начальные значения для анимации
        Vector3 startPosition = currentDetail.position;
        Quaternion startRotation = currentDetail.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            // Линейная интерполяция позиции и вращения
            currentDetail.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / rotationDuration);
            currentDetail.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / rotationDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Убедимся, что деталь точно встала на место
        currentDetail.position = targetPosition;
        currentDetail.rotation = targetRotation;

        // Отключаем физику и взаимодействие после завершения
        if (currentDetailRigidbody != null)
        {
            currentDetailRigidbody.useGravity = false;
            currentDetailRigidbody.isKinematic = true;
        }

        currentDetailMeshCollider.enabled = false;
        isSnapped = true;

        // Сообщаем о завершении сборки текущей детали
        if (gearboxAssembly != null)
        {
            gearboxAssembly.OnDetailSnapped();
        }
    }
}
