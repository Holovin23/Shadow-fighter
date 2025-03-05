using System.Collections.Generic;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Stats
{
    public static class StatCalculator
    {
        public static float CalculateModifiedStat(float baseValue, Vector2 minMaxValue, IEnumerable<StatModifier> modifiers)
        {
            float totalValue = baseValue;

            foreach (var modifier in modifiers)
            {
                if (modifier.modifierType == StatModifierType.Flat)
                    totalValue += modifier.value;
            }
            
            foreach (var modifier in modifiers)
            {
                if (modifier.modifierType == StatModifierType.Multiplier)
                    totalValue *= modifier.value;
            }
            
            foreach (var modifier in modifiers)
            {
                if (modifier.modifierType == StatModifierType.GlobalMultiplier)
                    totalValue *= modifier.value;
            }
            
            return Mathf.Clamp(totalValue, minMaxValue.x, minMaxValue.y);
        }
    }
}