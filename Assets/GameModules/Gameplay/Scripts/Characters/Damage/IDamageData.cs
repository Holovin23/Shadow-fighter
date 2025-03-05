using System.Collections.Generic;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Damage
{
    public interface IDamageData
    {
        public Transform GetInteractor();
        public float GetDamageValue();
        public List<DamageEffect> GetDamageEffects();
        public bool IsCriticalDamage();
        public Transform GetDamagePoint(); 
    }
}