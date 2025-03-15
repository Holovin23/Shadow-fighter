using System;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    [Serializable]
    public class InteractionData : IInteractionData
    {
        private Transform interactor;
        private Transform target;

        public InteractionData()
        {
        }

        public InteractionData(Transform interactor, Transform target)
        {
            this.interactor = interactor;
            this.target = target;
        }

        public Transform GetInteractor() => 
            interactor;

        public Transform GetTarget() =>
            target;

  
    }
}