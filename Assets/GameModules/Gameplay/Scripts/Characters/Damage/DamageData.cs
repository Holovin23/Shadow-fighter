using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters.Damage
{
    public class DamageData : IDamageData
    {
        public Transform interactor;
        public float damageValue;
        public Transform damagePoint;
        public List<DamageEffect> damageEffects = new List<DamageEffect>();

        public DamageData()
        {
        }

        public DamageData(Transform interactor, float damageValue)
        {
            this.interactor = interactor;
            this.damageValue = damageValue;
        }

        public DamageData(DamageData damageData)
        {
            interactor = damageData.interactor;
            damageValue = damageData.damageValue;
            damagePoint = damageData.damagePoint;
            damageEffects = damageData.damageEffects.Select(effect=> new DamageEffect(effect.type, effect.value)).ToList();
        }

        public void AddDamagePoint(Transform damagePoint) =>
            this.damagePoint = damagePoint;

        public Transform GetInteractor() =>
            interactor;

        public float GetDamageValue()
        {
            var criticalDamageEffect = GetDamageEffect(DamageEffectType.Critical);
            if (criticalDamageEffect != default)
                return damageValue * criticalDamageEffect.value;
            return damageValue;
        }

        public List<DamageEffect> GetDamageEffects() =>
            damageEffects;

        public DamageEffect GetDamageEffect(DamageEffectType effectType) =>
            damageEffects.FirstOrDefault(x => x.type == effectType);

        public bool IsCriticalDamage() =>
            GetDamageEffect(DamageEffectType.Critical) != default;

        public Transform GetDamagePoint() =>
            damagePoint;

        public void SetDamageMultiplier(float multiplier) => 
            damageValue = Mathf.RoundToInt(damageValue * multiplier);

        public void DisableCriticalDamage()
        {
           var criticalDamageEffect = damageEffects.FirstOrDefault(x => x.type == DamageEffectType.Critical);
           if (criticalDamageEffect != null)
               damageEffects.Remove(criticalDamageEffect);
        } 
    }
}