using System.Collections.Generic;
using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    public interface IProjectile
    {
        public Transform Owner { get; }
        public List<ProjectileAttribute> Attributes { get; }
        public ProjectileSpawnType SpawnType { get; }

        public void OnSpawn(IInteractionData interactionData);
        public void OnCollision(GameObject gameObject);
        public void OnDespawn();

        public void SetAttributes(List<ProjectileAttribute> attributes);
        public void EnableCollisions();
        public void DisableCollisions();
    }
}