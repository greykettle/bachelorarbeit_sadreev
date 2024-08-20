using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchDetail : MonoBehaviour
{
    public int currentLevel;
    public GameObject instrument;
    private MeshCollider currentDetailMeshCollider;
    private GearboxAssembly gearboxAssembly;

    void Start()
    {
        // Попробуем найти GearboxAssembly в сцене
        gearboxAssembly = FindObjectOfType<GearboxAssembly>();

        

        currentDetailMeshCollider = this.GetComponent<MeshCollider>();
    }

    void Update()
    {
        // Проверка активации коллайдера в зависимости от шага сборки
        if (gearboxAssembly != null && currentLevel == gearboxAssembly.CurrentStep && !currentDetailMeshCollider.enabled)
        {
            currentDetailMeshCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что инструмент активировал триггер
        if (other.gameObject == instrument)
        {
            Debug.Log("We collided");

            if (gearboxAssembly != null)
            {
                gearboxAssembly.OnDetailSnapped();
            }
            else
            {
                Debug.LogWarning("GearboxAssembly не установлен.");
            }

            currentDetailMeshCollider.enabled = false; // Отключаем коллайдер после срабатывания триггера
        }
    }
}

