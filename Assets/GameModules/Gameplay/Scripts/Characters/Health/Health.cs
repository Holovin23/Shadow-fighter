using System;
using GameModules.Gameplay.Scripts.Characters.Stats;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Health
{
    public class Health : MonoBehaviour, IHealthProvider
    {
        public event Action<IHealthProvider> OnDeathValue;
        public event Action<IHealthProvider, float> OnValueChanged;
        public event Action<IHealthProvider, float> OnValueAdded;
        public event Action<IHealthProvider, float> OnValueRemoved;
        public float MaxValue { get; private set; }
        public float Value { get; private set; }
        
        public void Initialize(Stat healthStat)
        {
            MaxValue = healthStat.Value;
            Value = (int)healthStat.Value;
            Debug.Log($"Health: {Value}");
        }

        public void Add(float value)
        {
            Value += value;
            Value = Mathf.Clamp(Value, 0f, MaxValue);
            OnValueAdded?.Invoke(this, value);
            OnValueChanged?.Invoke(this, Value);
        }

        public void Remove(float value)
        {
            if (Value == 0)
                return;

            Value -= value;
            Value = Mathf.Clamp(Value, 0, MaxValue);

            OnValueRemoved?.Invoke(this, value);
            OnValueChanged?.Invoke(this, Value);
            Debug.Log($"Health: {Value}");
            if (IsDeadlyValue())
                OnDeathValue?.Invoke(this);
        }

        public void SetValue(float value)
        {
            Value = value;
            Value = Mathf.Clamp(Value, 0f, MaxValue);
            OnValueChanged?.Invoke(this, Value);
        }

        public void SetMaxValue(float value)
        {
            MaxValue = value;
        }

        private bool IsDeadlyValue() =>
            Value <= 0;
    }
}