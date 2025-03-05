using System;

namespace GameModules.Gameplay.Scripts.Characters.Damage
{
    [Serializable]
    public class DamageEffect
    {
        public DamageEffectType type;
        public float value;

        public DamageEffect(DamageEffectType type, float value)
        {
            this.type = type;
            this.value = value;
        }
    }
    
    [Serializable]
    public enum DamageEffectType
    {
        None = 0,
        Critical = 1,
        Fire = 5,
        Frost = 10,
        Poison = 15,
    }
    
}