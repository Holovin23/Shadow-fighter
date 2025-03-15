using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectileLineForceAttribute), fileName = nameof(ProjectileLineForceAttribute))]
    public class ProjectileLineForceAttribute : ProjectileAttribute
    {
        [SerializeField] private float speedValue;
        
        private Rigidbody rigidbody;

        public override void OnSpawn(IInteractionData interactionData)
        {
            if (projectile.Owner.TryGetComponent(out rigidbody))
            {
                var direction = projectile.Owner.forward;
                direction.y = 0f; 
                direction = direction.normalized;
                rigidbody.AddForce(direction * speedValue, ForceMode.VelocityChange);
            }
            else
                Debug.LogError("Something wrong: Projectile hasn't component Rigidbody.");
        }

        public override void OnDespawn()
        {
            if (rigidbody != null)
                rigidbody.velocity = Vector3.zero;

            base.OnDespawn();
        }

        public override void AcceptVisitor(IProjectileAttributesVisitor visitor) =>
            visitor.Visit(this);

        public void SetSpeedValue(float value) =>
            speedValue = value;

    }
}