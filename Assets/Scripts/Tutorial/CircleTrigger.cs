using UnityEngine;

public class CircleTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager;
    public bool isFirstCircle = true;

    void OnTriggerEnter(Collider other)
    {
        if (isFirstCircle)
            {
                tutorialManager.OnFirstCircleEntered();
            }
            else
            {
                tutorialManager.OnSecondCircleEntered();
            }
    }
}
