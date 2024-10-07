using HighlightPlus;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts.Demontage
{
    [Serializable]
    public class OnStepPreparing
    {
        [field: SerializeField] public int stepIndex { get; private set; }
        [field: SerializeField] public UnityEvent OnDetailPrepaingByStepIndex { get; private set; }

    }

    public class Detail : MonoBehaviour
    {
        public DetailType DetailType;
        public bool isDemontagedAndSorted = false;
        public bool isNeedToEnableCollidersInChildrens = true;

        public UnityEvent OnDetailPreparing;
        [SerializeField] OnStepPreparing[] preparingsByStep;

        private Collider[] colliders;
        private Collider[] collidersInChildren;
        private Rigidbody rigidbody;
        private XRGrabInteractable interactable;
        private HighlightEffect highlightEffect;
        private bool isWrenched = false;

        public bool isNeedToWrench = false;
        public GameObject instrument;
        public Image circleTimer;
        public float wrenchTime;
        public float runningTime;
        [HideInInspector] public Step step;
        private void Awake()
        {
            colliders = GetComponents<Collider>();
            highlightEffect = GetComponent<HighlightEffect>();
            collidersInChildren = GetComponentsInChildren<Collider>();
            rigidbody = GetComponent<Rigidbody>();
            interactable = GetComponent<XRGrabInteractable>();

            interactable.selectExited.AddListener(OnDropByHand);
        }
        private void OnDestroy()
        {
            interactable.selectExited.RemoveListener(OnDropByHand);
        }
        public void OnSort()
        {
            isDemontagedAndSorted = true;
            highlightEffect.SetHighlighted(false);
            interactable.enabled = false;
            if (step != null)
                step.OnDetailSotringInStepByStep();
            transform.SetParent(null);

        }

        public void OnLaunchPreparing()
        {
            
            //enable colliders in children
            if (isNeedToEnableCollidersInChildrens == true)
            {
                for (int i = 0; i < collidersInChildren.Length; i++)
                {
                    collidersInChildren[i].enabled = true;
                }
            }

            //enable colliders on detail
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }

            highlightEffect.SetHighlighted(true);

            if (isNeedToWrench == false)
            {
                interactable.enabled = true;
            }
            else
            {
                interactable.enabled = false;
            }

            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            OnDetailPreparing?.Invoke();
            if (preparingsByStep.Length > 0)
            {
                preparingsByStep
                    .First(x => x.stepIndex == DemontageManager.indexCurrentStep)
                    .OnDetailPrepaingByStepIndex?.Invoke();
            }
        }


        public void OnDropByHand(SelectExitEventArgs args)
        {
            //Enable gravity, disable isKinematic
            Debug.Log($"OnDropByHand on detail {gameObject.name}");
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isNeedToWrench && !isWrenched)
            {
                if (other.gameObject == instrument)
                {
                    circleTimer.gameObject.SetActive(true);
                }
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (isNeedToWrench && !isWrenched)
            {
                if (other.gameObject == instrument)
                {
                    runningTime += Time.deltaTime;
                    circleTimer.fillAmount = runningTime / wrenchTime;
                    if (runningTime >= wrenchTime)
                    {
                        Debug.Log("We collided");
                        interactable.enabled = true;
                        isWrenched = true;
                        ResetCircle();
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (isNeedToWrench && !isWrenched)
            {
                if (other.gameObject == instrument)
                {
                    ResetCircle();
                    runningTime = 0;
                }
            }
        }
        private void ResetCircle()
        {
            circleTimer.fillAmount = 0;
            circleTimer.gameObject.SetActive(false);
        }
    }


}