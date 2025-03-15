using System;
using GameModules.Gameplay.Scripts.Characters.Damage;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Health
{
    public class Damageable: MonoBehaviour
    {
        public event Action<Damageable, IDamageData> OnHit;

        [SerializeField] private float damageMultiplier = 1f;
        [SerializeField] private bool isCriticalDamageEnabled = true;

        public void SetDamageMultiplier(float damageMultiplier) => 
            this.damageMultiplier = damageMultiplier;

        public void SetCriticalDamageEnable(bool enabled) =>
            isCriticalDamageEnabled = enabled;

        public void HandleDamage(IDamageData damageData)
        {
            var damageDataCopy = damageData as DamageData;
            damageDataCopy.SetDamageMultiplier(damageMultiplier);
            
            if(!isCriticalDamageEnabled)
             damageDataCopy.DisableCriticalDamage();

            OnHit?.Invoke(this, damageDataCopy);
        }
    }
}