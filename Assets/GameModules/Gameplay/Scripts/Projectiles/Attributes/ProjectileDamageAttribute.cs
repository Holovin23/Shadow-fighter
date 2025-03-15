using GameModules.Gameplay.Scripts.Characters;
using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Health;
using ModestTree;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectileDamageAttribute), fileName = nameof(ProjectileDamageAttribute))]
    public class ProjectileDamageAttribute : ProjectileAttribute
    {
        private IDamageData damageData;

        public override void OnCollision(IInteractionData interactionData)
        {
            if (!interactionData.GetTarget().TryGetComponent(out Damageable damageable))
                return;
            
            damageable.HandleDamage(damageData);
            Debug.Log("DAMAGE DEALT TO ENEMY " + damageData.GetDamageValue());
        }

        public override void AcceptVisitor(IProjectileAttributesVisitor visitor) =>
            visitor.Visit(this);

        public void SetDamageData(IDamageData damageData) => 
            this.damageData = damageData;
    }
}