using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class StayColliderTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask interactionLayer;

        public event System.Action<Collider> OnInteraction;

        private void OnTriggerStay(Collider other)
        {
            if (interactionLayer.Includes(other.gameObject.layer))
                OnInteraction?.Invoke(other);
            
        }
    }
}