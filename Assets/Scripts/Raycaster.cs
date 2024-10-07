using DrawXXL;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Raycaster : MonoBehaviour
    {
        [SerializeField] Transform RaycastOrigin;
        [SerializeField] float RayLength;
        [SerializeField] LayerMask interactableLayer;
        // Update is called once per frame
        void Update()
        {
            Ray ray = new Ray(RaycastOrigin.position, RaycastOrigin.forward);
            Debug.DrawRay(ray.origin, ray.direction * RayLength, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, RayLength, interactableLayer))
            {
                Debug.Log($"We see object {hitInfo.collider.name}");

                if (hitInfo.collider.gameObject.TryGetComponent(out TagDrawer drawer))
                {
                    drawer.enabled = true;
                }
            }
        }
    }
}