using UnityEngine;
using UnityEngine.UI;

public class WrenchBolts : MonoBehaviour
{
    public float wrenchTime;
    public float runningTime;
    public Image circleTimer;
    public WrenchBoltManager manager;  
    public GameObject instrument;    
    private bool isSnapped = false;

    private void Start()
    {
        GetComponent<Rigidbody>().sleepThreshold = 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isSnapped && other.gameObject == instrument)
        {
            Debug.Log($"Instrument {instrument.name} entered in bolt {gameObject.name}");
            circleTimer.gameObject.SetActive(true);
        }
    }


    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject == instrument)
        {
            runningTime += Time.deltaTime;
            circleTimer.fillAmount = runningTime / wrenchTime;
            if (runningTime >= wrenchTime)
            {
                Debug.Log("We collided");
                isSnapped = true;
                manager.DetailSnapped(this.gameObject);
                Debug.Log(gameObject.name + instrument.name);
                this.gameObject.GetComponent<MeshCollider>().enabled = false;

                ResetCircle();
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
