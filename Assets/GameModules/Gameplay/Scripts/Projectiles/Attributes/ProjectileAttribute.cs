using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectileAttribute), fileName = nameof(ProjectileAttribute))]
    public abstract class ProjectileAttribute : ScriptableObject
    {
        protected IProjectile projectile;

        public virtual void Initialize(IProjectile projectile) =>
            this.projectile = projectile;

        public virtual void OnSpawn(IInteractionData interactionData)
        {
        }

        public virtual void OnCollision(IInteractionData interactionData)
        {
        }

        public virtual void OnDespawn()
        {
        }

        public virtual void AcceptVisitor(IProjectileAttributesVisitor visitor) => 
            visitor.Visit(this);
    }
}