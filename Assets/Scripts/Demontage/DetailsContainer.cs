using UnityEngine;

namespace Assets.Scripts.Demontage
{
    public class DetailsContainer : MonoBehaviour
    {
        public DetailType DetailType;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Detail detail) && DetailType == detail.DetailType)
            {
                detail.OnSort();
            }
        }
    }
}