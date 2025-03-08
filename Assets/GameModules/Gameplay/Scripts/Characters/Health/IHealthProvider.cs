using System;

namespace GameModules.Gameplay.Scripts.Characters.Health
{
    public interface IHealthProvider
    {
        public event Action<IHealthProvider> OnDeathValue;
        public event Action<IHealthProvider, float> OnValueChanged;
        public event Action<IHealthProvider, float> OnValueAdded;
        public event Action<IHealthProvider, float> OnValueRemoved;

        public float MaxValue { get; }
        public float Value { get; }

        public void Add(float value);
        public void Remove(float value);
        public void SetValue(float value);
        public void SetMaxValue(float value);
    }
}