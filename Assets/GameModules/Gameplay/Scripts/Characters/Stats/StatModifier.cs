using System;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Stats
{
    [Serializable]
    public class StatModifier
    {
        public StatType statType;
        public StatModifierType modifierType;
        public float value;
        public string source;

        public StatModifier(StatType statType, float value, StatModifierType modifierType = StatModifierType.Flat, string source = null)
        {
            this.statType = statType;
            this.value = value;
            this.modifierType = modifierType;
            this.source = source;
        }

        public StatModifier(StatModifier modifier)
        {
            statType = modifier.statType;
            value = modifier.value;
            modifierType = modifier.modifierType;
            source = modifier.source;
        }

        public override bool Equals(object obj)
        {
            if (obj is not StatModifier modifier)
                return false;
            if (statType != modifier.statType)
                return false;
            if (modifierType != modifier.modifierType)
                return false;
            if (source != modifier.source)
                return false;
            if (Mathf.Abs(value - modifier.value) > 0.0001)
                return false;
            return true;
        }
    }
}