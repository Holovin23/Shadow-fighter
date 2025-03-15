using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectileCollisionsAttribute),
        fileName = nameof(ProjectileCollisionsAttribute))]
    public class ProjectileCollisionsAttribute : ProjectileAttribute
    {
        [SerializeField] private int collisionsCount = 1;

        public override void OnCollision(IInteractionData interactionData)
        {
            collisionsCount -= 1;

            if (collisionsCount < 1)
                projectile.OnDespawn();
        }
        
        public override void AcceptVisitor(IProjectileAttributesVisitor visitor) =>
            visitor.Visit(this);

        public void SetCollisionsCount(int count) => 
            collisionsCount = count;
    }
}