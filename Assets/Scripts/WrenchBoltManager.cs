using System.Collections.Generic;
using UnityEngine;

public class WrenchBoltManager : MonoBehaviour
{
    public List<GameObject> details;
    private HashSet<GameObject> snappedDetails = new HashSet<GameObject>();  // Хранит закрученные детали
    private GearboxAssembly gearboxAssembly;

    void Start()
    {

        gearboxAssembly = FindObjectOfType<GearboxAssembly>();
        foreach (GameObject detail in details)
        {
            WrenchBolts trigger = detail.GetComponent<WrenchBolts>();
            if (trigger != null)
            {
                trigger.manager = this;  // Устанавливаем ссылку на менеджер для каждой детали
            }
            else
            {
                Debug.LogError("Деталь " + detail.name + " не имеет скрипта DetailTrigger.");
            }
        }
    }

    public void DetailSnapped(GameObject detail)
    {
        if (!snappedDetails.Contains(detail))
        {
            snappedDetails.Add(detail);
        }

        // Проверяем, все ли детали были закручены
        if (snappedDetails.Count == details.Count)
        {
            Debug.Log("Все детали закручены. Переход на следующий уровень.");
            if (gearboxAssembly != null)
            {
                gearboxAssembly.OnDetailSnapped();  // Здесь вызывается переход на следующий уровень
            }
        }
    }
}
