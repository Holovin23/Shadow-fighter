using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Stats
{
    [Serializable]
    public class Stat
    {
        public event Action<Stat, float> OnValueChange;

        [SerializeField] private StatType type;
        [SerializeField] private float baseValue;
        [SerializeField] private Vector2 minMaxValue = new Vector2(0f, int.MaxValue);

        private List<StatModifier> modifiers = new List<StatModifier>();

        private float totalValue;

        public float Value
        {
            get
            {
                UpdateTotalValue();
                return totalValue;
            }
            set => totalValue = value;
        }

        public StatType Type => type;
        public float BaseValue => baseValue;
        private IEnumerable<StatModifier> Modifiers => modifiers;

        public Stat(Stat stat)
        {
            type = stat.type;
            baseValue = stat.baseValue;
            minMaxValue = stat.minMaxValue;
            modifiers = new List<StatModifier>();

            if (stat.modifiers != null)
            {
                foreach (var modifier in stat.modifiers)
                    modifiers.Add(new StatModifier(modifier));
            }
        }

        public Stat(StatType type, float baseValue)
        {
            this.type = type;
            this.baseValue = baseValue;
        }

        public void AddModifier(StatModifier statModifier)
        {
            modifiers.Add(statModifier);
            UpdateTotalValue();
        }

        public void RemoveModifier(StatModifier statModifier)
        {
            for (var index = modifiers.Count - 1; index >= 0; index--)
            {
                var modifier = modifiers[index];
                if (modifier.Equals(statModifier))
                {
                    modifiers.Remove(statModifier);
                    UpdateTotalValue();
                    return;
                }
            }
        }

        public void RemoveAllModifiersWithSource(string source)
        {
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                if (modifiers[i].source == source)
                    modifiers.Remove(modifiers[i]);
            }

            UpdateTotalValue();
        }

        public void RemoveAllModifiers() =>
            modifiers.Clear();

        public void UpdateTotalValue()
        {
            var oldValue = totalValue;
            var newValue = StatCalculator.CalculateModifiedStat(baseValue, minMaxValue, modifiers);
            Value = newValue;

            if (Mathf.Abs(newValue - oldValue) > 0.01f)
                OnValueChange?.Invoke(this, totalValue);
        }

        public bool IsMaxValueReached() =>
            Value >= minMaxValue.y;
    }
}