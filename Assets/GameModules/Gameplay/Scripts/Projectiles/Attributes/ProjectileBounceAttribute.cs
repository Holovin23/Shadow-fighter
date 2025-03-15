using System.Collections.Generic;
using System.Linq;
using GameModules.Gameplay.Scripts.Characters;
using GameModules.Gameplay.Scripts.Characters.Damage;
using Pooling;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.Projectiles
{
   [CreateAssetMenu(menuName = DataPath.ProjectileAttributes + nameof(ProjectileBounceAttribute),
        fileName = nameof(ProjectileBounceAttribute))]
    public class ProjectileBounceAttribute : ProjectileAttribute
    {
        [Inject] IPoolService poolableManager;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField, BoxGroup("Settings")] private List<ProjectileSpawnType> availableSpawnTypes = new List<ProjectileSpawnType>();
        [SerializeField, BoxGroup("Settings")] private int bounceCount = 1;
        [SerializeField, BoxGroup("Settings")] private float bounceRadius = 10;
        [SerializeField, BoxGroup("Settings")] private float damageMultiplier = 0.5f;
        [SerializeField, BoxGroup("Settings")] private bool isReducedCountAfterCollision;
        [SerializeField, BoxGroup("Settings")] private LayerMask collisionLayerMask;
        

        private Projectile spawnedProjectile;
        private DamageData damageData;

        public override void OnCollision(IInteractionData interactionData)
        {
            if (!IsWork())
                return;

            ModifyDamageData();

            for (int index = 0; index < bounceCount; index++)
                CreateBounceProjectile(interactionData);

            if (isReducedCountAfterCollision)
                bounceCount = 0;
        }

        private bool IsWork()
        {
            if (availableSpawnTypes.Count == 0)
                return false;
            if (!availableSpawnTypes.Contains(projectile.SpawnType))
                return false;
            if (bounceCount == 0)
                return false;
            return true;
        }

        private void ModifyDamageData()
        {
            damageData.SetDamageMultiplier(damageMultiplier);
            damageData.DisableCriticalDamage();
        }

        private void CreateBounceProjectile(IInteractionData interactionData)
        {
            var interactor = interactionData.GetTarget();
            var target = GetTarget(new List<Transform> { interactor, interactor.parent });
            var newInteractionData = new InteractionData(interactor, target);

            spawnedProjectile = SpawnProjectile();
            SetupAttributes(spawnedProjectile);
            spawnedProjectile.DisableCollisions();
            spawnedProjectile.OnSpawn(newInteractionData);
        }

        private void SetupAttributes(Projectile projectile)
        {
            var attributes = projectile.Attributes.Select(attribute => attribute.Copy()).ToList();

            foreach (var attribute in attributes)
            {
                attribute.Initialize(projectile);

                if (attribute is ProjectileDamageAttribute damageAttribute)
                    damageAttribute.SetDamageData(damageData);
            }

            projectile.SetAttributes(attributes);
        }

        private Transform GetTarget(List<Transform> excludeTargets)
        {
            Transform target = null;
            var colliders = Physics.OverlapSphere(projectile.Owner.position, bounceRadius, collisionLayerMask);
            colliders.Shuffle();

            foreach (var collider in colliders)
            {
                if (excludeTargets.Contains(collider.transform) || excludeTargets.Contains(collider.transform.parent))
                    continue;

                var collision = collider.transform.parent != null ? collider.transform.parent : collider.transform;
                {
                    if (collision.TryGetComponent(out Character character))
                    {
                        target = character.transform;
                        break;
                    }
                }
            }

            return target;
        }

        private Projectile SpawnProjectile()
        {
            return poolableManager.Spawn(projectilePrefab, projectile.Owner.position, Quaternion.Euler(Vector3.up));
        }

        public override void AcceptVisitor(IProjectileAttributesVisitor visitor) =>
            visitor.Visit(this);

        public void SetBounceCount(int count) =>
            bounceCount = count;

        public void SetDamageData(IDamageData damageData) =>
            this.damageData = new DamageData(damageData as DamageData);
    }
}