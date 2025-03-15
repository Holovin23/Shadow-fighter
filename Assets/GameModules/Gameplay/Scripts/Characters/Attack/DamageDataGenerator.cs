using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Stats;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class DamageDataGenerator : IDamageGenerator
    {

        public DamageData Generate(IStatsProvider statsProvider, IInteractionData interactionData)
        {
            float damageValue = GetDamageValue(statsProvider);
            DamageData damageData = new DamageData(interactionData.GetInteractor(), damageValue);

            AddPossibleEffects(damageData, statsProvider);

            return damageData;
        }

        /*private Transform GetDamagePoint(Transform target, DamageData damageData)
        {
            if (target.TryGetComponent(out IHitPointsOwner hitPointsOwner))
                return hitPointsOwner.GetHitPoint(damageData);
            if (target.TryGetComponent(out IInteractionTarget interactionTarget))
                return interactionTarget.GetInteractionPoint();
            return target;
        }*/

        private float GetDamageValue(IStatsProvider statsProvider)
        {
            var damageValue = 0f;

            if (statsProvider.TryGetStat(StatType.AttackDamage, out var damageStat))
                damageValue += damageStat.Value;

            return damageValue;
        }
        
        private void AddPossibleEffects(DamageData damageData,IStatsProvider statsProvider)
        {
            TryAddCriticalAttackEffect(damageData,statsProvider);
            TryAddFireAttackEffect(damageData,statsProvider);
            TryAddFrostAttackEffect(damageData,statsProvider);
            TryAddPoisonAttackEffect(damageData,statsProvider);
        }
        
        private void TryAddFireAttackEffect(DamageData damageData,IStatsProvider statsProvider)
        {
            var effectValue = 0f;

            if (statsProvider.TryGetStat(StatType.FireAttackEffect, out var fireAttackStat))
                effectValue += fireAttackStat.Value;

            if (effectValue < 0.01f)
                return;

            damageData.damageEffects.Add(new DamageEffect(DamageEffectType.Fire, effectValue));
        }
        
        private void TryAddPoisonAttackEffect(DamageData damageData,IStatsProvider statsProvider)
        {
            var effectValue = 0f;

            if (statsProvider.TryGetStat(StatType.PoisonAttackEffect, out var poisonAttackStat))
                effectValue += poisonAttackStat.Value;
            
            if (effectValue < 0.01f)
                return;

            damageData.damageEffects.Add(new DamageEffect(DamageEffectType.Poison, effectValue));
        }

        private void TryAddFrostAttackEffect(DamageData damageData,IStatsProvider statsProvider)
        {
            var effectValue = 0f;

            if (statsProvider.TryGetStat(StatType.FrostAttackEffect, out var frostAttackStat))
                effectValue += frostAttackStat.Value;

            if (effectValue < 0.01f)
                return;

            damageData.damageEffects.Add(new DamageEffect(DamageEffectType.Frost, effectValue));
        }
        
        private void TryAddCriticalAttackEffect(DamageData damageData,IStatsProvider statsProvider)
        {
            var criticalChance = 0f;

            if (statsProvider.TryGetStat(StatType.CriticalChance, out var criticalChanceStat))
                criticalChance += criticalChanceStat.Value;

            bool isCriticalDamage = criticalChance > Random.value;
            
            if(!isCriticalDamage)
                return;
            
            var criticalDamageMultiplier = 1f;

            if (statsProvider.TryGetStat(StatType.CriticalDamage, out var criticalDamageStat))
                criticalDamageMultiplier += criticalDamageStat.Value;
            
            var criticalDamageValue =  Mathf.Max(1f, criticalDamageMultiplier);
            
            damageData.damageEffects.Add(new DamageEffect(DamageEffectType.Critical, criticalDamageValue));
        }

    }
}