using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WrenchDetail : MonoBehaviour
{
    public float wrenchTime;
    public float runningTime;
    public int currentLevel;
    public GameObject instrument;
    private MeshCollider currentDetailMeshCollider;
    private GearboxAssembly gearboxAssembly;
    public Image circleTimer;


    void Start()
    {

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        GetComponent<Rigidbody>().sleepThreshold = 0;


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
        if (other.gameObject == instrument)
        {
            circleTimer.gameObject.SetActive(true);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // Проверяем, что инструмент активировал триггер
        if (other.gameObject == instrument)
        {
            runningTime += Time.deltaTime;
            circleTimer.fillAmount = runningTime / wrenchTime;
            if (runningTime >= wrenchTime)
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

                ResetCircle();
                currentDetailMeshCollider.enabled = false;
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == instrument)
        {
            ResetCircle();
            runningTime = 0;
        }
    }

    private void ResetCircle()
    {
        circleTimer.fillAmount = 0;
        circleTimer.gameObject.SetActive(false);
    }
}