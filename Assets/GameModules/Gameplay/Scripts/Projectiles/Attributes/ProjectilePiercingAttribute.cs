using GameModules.Gameplay.Scripts.Characters;
using GameModules.Gameplay.Scripts.Characters.Damage;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectilePiercingAttribute), fileName = nameof(ProjectilePiercingAttribute))]
    public class ProjectilePiercingAttribute : ProjectileAttribute
    {
        [SerializeField] private float damageMultiplierPerCollision = 0.8f;

        private DamageData damageData;

        public override void OnCollision(IInteractionData interactionData)
        {
            damageData.DisableCriticalDamage();
            damageData.SetDamageMultiplier(damageMultiplierPerCollision);
        }

        public override void AcceptVisitor(IProjectileAttributesVisitor visitor) =>
            visitor.Visit(this);

        public void SetDamageData(DamageData damageData) =>
            this.damageData = damageData;
    }
}