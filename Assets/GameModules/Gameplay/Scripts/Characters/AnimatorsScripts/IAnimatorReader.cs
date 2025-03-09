using System;

namespace GameModules.Gameplay.Scripts.Characters.AnimatorsScripts
{
    public interface IAnimatorReader
    {
        public event Action OnAttackStarted;
        public event Action OnAttackReleased;
        public event Action OnAttackEnded;
    }
}