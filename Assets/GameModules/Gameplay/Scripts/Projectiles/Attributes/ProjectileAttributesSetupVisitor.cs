using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Stats;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    public class ProjectileAttributesSetupVisitor : IProjectileAttributesVisitor
    {
        private readonly IStatsProvider statsProvider;
        private DamageData damageData;

        public ProjectileAttributesSetupVisitor(IStatsProvider statsProvider)
        {
            this.statsProvider = statsProvider;
        }

        public void SetDamageData(IDamageData damageData) =>
            this.damageData = damageData as DamageData;

        public void Visit(ProjectileAttribute effect)
        {
        }

        public void Visit(ProjectileDamageAttribute attribute)
        {
            damageData = new DamageData(damageData);

            var criticalChance = 0f;

            if (statsProvider.TryGetStat(StatType.CriticalChance, out var criticalChanceStat))
                criticalChance = criticalChanceStat.Value;

            bool isCriticalDamage = criticalChance > Random.value;

            var criticalDamageMultiplier = 1f;

            if (statsProvider.TryGetStat(StatType.CriticalDamage, out var criticalDamageStat))
                criticalDamageMultiplier += criticalDamageStat.Value;

            var criticalDamageValue = Mathf.Max(1f, criticalDamageMultiplier);

            if (isCriticalDamage)
            {
                if(!damageData.IsCriticalDamage())
                    damageData.damageEffects.Add(new DamageEffect(DamageEffectType.Critical, criticalDamageValue));
            }
            else
                damageData.DisableCriticalDamage();

            attribute.SetDamageData(damageData);
        }

        public void Visit(ProjectileBounceAttribute attribute)
        {
            attribute.SetDamageData(damageData);

            if (statsProvider.TryGetStat(StatType.ProjectileBounce, out var bounceStat))
                attribute.SetBounceCount((int)bounceStat.Value);
        }

        public void Visit(ProjectilePiercingAttribute attribute) => 
            attribute.SetDamageData(damageData);

        public void Visit(ProjectileDirectionForceAttribute attribute)
        {
            if (statsProvider.TryGetStat(StatType.ProjectileSpeed, out var projectileSpeedStat))
                attribute.SetSpeedValue(projectileSpeedStat.Value);

            attribute.SetTargetPoint(damageData.GetDamagePoint());
        }

        public void Visit(ProjectileLineForceAttribute attribute)
        {
            if (statsProvider.TryGetStat(StatType.ProjectileSpeed, out var projectileSpeedStat))
                attribute.SetSpeedValue(projectileSpeedStat.Value);
        }

        public void Visit(ProjectileCollisionsAttribute attribute)
        {
            var collisionCount = 1;

            if (statsProvider.TryGetStat(StatType.ProjectilePiercing, out var penetrationsStat))
                collisionCount += (int)penetrationsStat.Value;

            attribute.SetCollisionsCount(collisionCount);
        }
    }
}