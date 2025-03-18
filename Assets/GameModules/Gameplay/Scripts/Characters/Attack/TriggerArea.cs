using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    [RequireComponent(typeof(Collider))]
    public class TriggerArea : MonoBehaviour
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;
        
        [SerializeField] private Collider trigger;
        [SerializeField] private bool useLayerFilter;
        [SerializeField, ShowIf(nameof(useLayerFilter))] private LayerMask targetLayer;

        public Collider Trigger => trigger;

        public void Disable()
        {
            trigger.enabled = false;
        }
        
        public void Enable()
        {
            trigger.enabled = true;
        }
        
        private void OnValidate()
        {
            trigger ??= GetComponent<Collider>();
            trigger.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsTargetedLayerFilter(other))
                OnEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsTargetedLayerFilter(other))
                OnExit?.Invoke(other);
        }

        private bool IsTargetedLayerFilter(Collider other) => 
            useLayerFilter && (targetLayer.value & (1 << other.gameObject.layer)) != 0;
    }
}