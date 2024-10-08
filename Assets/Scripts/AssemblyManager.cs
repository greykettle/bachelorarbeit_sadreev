using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;


public class AssemblyManager : MonoBehaviour
{
    private int step;
    private GameObject nextStepObject;
    public GameObject targetObject;
    private GameObject etalonModel;
    private GameObject firstStepObject;

    void checkStep()
    {
        int parentStep = this.gameObject.transform.parent.GetComponent<LevelManager>().step;
        if (step == parentStep) {
            GameObject parentNextStepObject = this.gameObject.transform.parent.GetComponent<LevelManager>().nextStepObject;
            if  (parentNextStepObject != null)
            {
                checkEtalon();
                nextStepObject = parentNextStepObject;
            }

        }

    }

    void checkEtalon()
    {
        etalonModel = this.gameObject.transform.parent.GetComponent<LevelManager>().etalonObject;
        Debug.Log("EtalonModel" + etalonModel);
    }
        
   public void setTargetObject() {
        checkStep();
        targetObject = nextStepObject;
    }

    public void assemblingModel()
    {

        Transform etalonPart = etalonModel.transform.Find(nextStepObject.name);
        if (etalonPart != null)
        {
            Debug.Log("assembly");
            nextStepObject.transform.SetParent(etalonPart.parent, true);

            nextStepObject.transform.localPosition = etalonPart.localPosition;
            nextStepObject.transform.localRotation = etalonPart.localRotation;


            nextStepObject.transform.SetParent(this.gameObject.transform, true);
     
        }


    }

    
}
