using System;
using System.Collections.Generic;
using GameModules.Gameplay.Scripts.Characters;
using ModestTree.Util;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour, IProjectile
    {
        [SerializeField] private TriggerArea triggerArea;
        [SerializeField] protected List<ProjectileAttribute> attributes = new List<ProjectileAttribute>();

        private IInteractionData interactionData;
        private List<Transform> collisions = new List<Transform>();

        private List<Transform> possibleCollisions = new List<Transform>();
        private List<Transform> ignoredCollisions = new List<Transform>();
        private Action<Projectile> OnDespawnAction;
        
        
        public Transform Owner => transform;
        public ProjectileSpawnType SpawnType { get; private set; }
        public List<ProjectileAttribute> Attributes => attributes;

        private void OnValidate()
        {
            triggerArea ??= GetComponentInChildren<TriggerArea>();
        }

        #region LifeCycle

        public void OnSpawn(IInteractionData interactionData)
        {
            this.interactionData = interactionData;

            triggerArea.OnEnter += TriggerArea_OnEnter;
            EnableCollisions();
            collisions = new List<Transform>();

            foreach (var attribute in attributes)
                attribute.OnSpawn(interactionData);
        }

        public void OnCollision(GameObject gameObject)
        {
            foreach (var attribute in attributes)
                attribute.OnCollision(interactionData);
        }

        public void OnDespawn()
        {
            triggerArea.OnEnter -= TriggerArea_OnEnter;

            foreach (var attribute in attributes)
                attribute.OnDespawn();

            OnDespawnAction?.Invoke(this);
            OnDespawnAction = null;
        }

        public void SetupDespawn(Action<Projectile> onDespawn = null)
        {
            OnDespawnAction = onDespawn;
        }
        #endregion
        
        public void EnableCollisions() => 
            triggerArea.Trigger.enabled = true;

        public void DisableCollisions() => 
            triggerArea.Trigger.enabled = false;

        public void SetAttributes(List<ProjectileAttribute> attributes) =>
            this.attributes = attributes;

        public void SetSpawnType(ProjectileSpawnType spawnType) => 
            SpawnType = spawnType;

        public void SetPossibleCollisions(List<Transform> possibleCollisions, List<Transform> ignoredCollisions)
        {
            this.ignoredCollisions = ignoredCollisions;
            this.possibleCollisions = possibleCollisions;
        }

        private void TriggerArea_OnEnter(Collider collider)
        {
            if (IsSelfCollision(collider))
                return;
            if (IsRepeatingCollision(collider))
                return;
            if (!HasPossibleCollision(collider))
                return;

            collisions.Add(GetCollisionOwner(collider.transform));
            interactionData = new InteractionData(interactionData.GetInteractor(), collider.transform);
            OnCollision(collider.gameObject);
        }

        private bool IsRepeatingCollision(Collider collider) => 
            collisions.Contains(GetCollisionOwner(collider.transform));

        private bool IsSelfCollision(Collider collider)
        {
            var isSelfCollision = false;
            var isParentSelfCollision = false;

            if (interactionData.GetInteractor() != null)
            {
                isSelfCollision = interactionData.GetInteractor() == collider.transform;
                isParentSelfCollision = GetCollisionOwner(interactionData.GetInteractor()) == GetCollisionOwner(collider.transform);
            }

            return isSelfCollision || isParentSelfCollision;
        }

        private bool HasPossibleCollision(Collider collider)
        {
            if (possibleCollisions.Count == 0)
                return true;
            
            return possibleCollisions.Contains(collider.transform) || possibleCollisions.Contains(GetCollisionOwner(collider.transform));
        }

        private Transform GetCollisionOwner(Transform target) =>
            target.transform.parent == default ? target.transform : target.transform.parent;
    }
}