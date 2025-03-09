using System;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.AnimatorsScripts
{
    public class CombatAnimationEventsReader : MonoBehaviour,IAnimatorReader
    {
        public event Action OnAttackStarted;
        public void InvokeOnAttackStarted() => OnAttackStarted?.Invoke();
        
        public event Action OnAttackReleased;
        public void InvokeOnAttackReleased() => OnAttackReleased?.Invoke();
        
        public event Action OnAttackEnded;
        public void InvokeOnAttackEnded() => OnAttackEnded?.Invoke();
    }
}