using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectileDirectionForceAttribute),
        fileName = nameof(ProjectileDirectionForceAttribute))]
    public class ProjectileDirectionForceAttribute : ProjectileAttribute
    {
        [SerializeField] private float speedValue;
        private Transform target;

        public override void OnSpawn(IInteractionData interactionData)
        {
            if (projectile.Owner.TryGetComponent(out Rigidbody rigidbody))
            {
                var direction = (target.position - projectile.Owner.position).normalized;
                rigidbody.AddForce(direction * speedValue, ForceMode.VelocityChange);
            }
            else
                Debug.LogError("Something wrong: Projectile hasn't component Rigidbody.");
        }

        public override void AcceptVisitor(IProjectileAttributesVisitor visitor) =>
            visitor.Visit(this);

        public void SetSpeedValue(float value) =>
            speedValue = value;

        public void SetTargetPoint(Transform target) =>
            this.target = target;
    }
}