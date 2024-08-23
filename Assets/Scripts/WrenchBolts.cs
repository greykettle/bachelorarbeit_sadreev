using UnityEngine;

public class WrenchBolts : MonoBehaviour
{
    public WrenchBoltManager manager;  // Ссылка на менеджер
    public GameObject Tool;    // Определенный инструмент, который должен взаимодействовать с деталью
    private bool isSnapped = false;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, если деталь еще не закручена и объект, с которым произошло столкновение, является нужным инструментом
        if (!isSnapped && other.gameObject == Tool)
        {
            isSnapped = true;
            manager.DetailSnapped(this.gameObject);
            Debug.Log(gameObject.name + Tool.name);
            this.gameObject.GetComponent<MeshCollider>().enabled = false;
        }
    }
}
