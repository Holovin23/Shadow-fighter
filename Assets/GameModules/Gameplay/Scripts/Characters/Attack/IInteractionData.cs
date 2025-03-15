using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    public interface IInteractionData
    {
        public Transform GetInteractor();
        public Transform GetTarget();
    }
}