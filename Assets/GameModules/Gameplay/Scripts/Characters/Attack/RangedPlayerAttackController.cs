using System.Collections;
using System.Collections.Generic;
using GameModules.Gameplay.Scripts.Characters;
using GameModules.Gameplay.Scripts.Characters.AnimatorsScripts;
using GameModules.Gameplay.Scripts.Characters.Damage;
using GameModules.Gameplay.Scripts.Characters.Stats;
using GameModules.Gameplay.Scripts.Factories;
using GameModules.Gameplay.Scripts.Projectiles;
using Pooling;
using UnityEngine;
using Zenject;

public class RangedPlayerAttackController : AttackController
{
        [Inject] private IColliderService _colliderService;
        [Inject] private IDamageGenerator _damageGeneratorService;
        [Inject] private IPoolService _poolService;
        [SerializeField] private float _searchEnemyRadius = 6f;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] public Projectile _projectilePrefab;
        
        private IAnimatorProvider _animatorProvider;
        private IAnimatorReader _animatorReader;
        private IMovementProvider _movementProvider;
        private float lastAttackTime;
        private bool _isReadyAttack;
        private Character _currentTarget;
        private Coroutine _updateRoutine;
        private Collider[] _resultArray = new Collider[100];
        private ProjectilesFactory projectilesFactory;
        
        private IEnumerator UpdateRoutine()
        {
            while (true)
            {
                if (!IsAvailableTarget(out _currentTarget))
                {
                    
                    _animatorProvider.SetCombat(false);
                    _movementProvider.SetTarget(null);
                    _movementProvider.SetCombatState(false);
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }
                
                _animatorProvider.SetCombat(true);
                _movementProvider.SetCombatState(true);
                
                if (Time.time - lastAttackTime >= _attackDelay )
                { 
                    _movementProvider.SetTarget(_currentTarget.transform);
                    lastAttackTime = Time.time;
                    _animatorProvider.StartAttack();
                }
                yield return new WaitForSeconds(0.25f);
            }
        }
        
        private void OnDestroy()
        {
            _animatorReader.OnAttackStarted -= EventsReader_OnAttackStarted;
            _animatorReader.OnAttackReleased -= EventsReader_OnAttackReleased;
            _animatorReader.OnAttackEnded -= EventsReader_OnAttackEnded;
        }
        
        public override void Initialize(Character owner)
        {
            base.Initialize(owner);
            _animatorProvider = owner.AnimatorProvider;
            _animatorReader = owner.AnimationEventsReader;
            var player = owner as PlayerCharacter;
            _movementProvider = player.PlayerMovementController;
            
            _animatorReader.OnAttackStarted += EventsReader_OnAttackStarted;
            _animatorReader.OnAttackReleased += EventsReader_OnAttackReleased;
            _animatorReader.OnAttackEnded += EventsReader_OnAttackEnded;
            _updateRoutine = StartCoroutine(UpdateRoutine());
        }

        private void Shoot()
        {
            var interactionData = new InteractionData(_ownerCharacter.Owner.transform, null);
            var damageData = _damageGeneratorService.Generate(_ownerCharacter.StatsProvider, interactionData);
            Attack(interactionData, damageData);
        }

        private bool IsAvailableTarget(out Character enemy)
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, _searchEnemyRadius, _resultArray, _enemyLayer);

            Character closestEnemy = null;
            enemy = null;
            float minSqrDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Character potentialEnemy;
                _colliderService.GetCharacter(_resultArray[i], out potentialEnemy);
                if (potentialEnemy == null) continue;

                float sqrDistance = (potentialEnemy.transform.position - transform.position).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closestEnemy = potentialEnemy;
                }
            }

            enemy = closestEnemy;
            return closestEnemy != null;
        }

        private void EnemyInMeleeRangeAttackRange(Collider obj)
        {
            if(!_isReadyAttack)
                return;
            
            lastAttackTime = Time.time;
            _isReadyAttack = false;

            _animatorProvider.StartAttack();
        }
        
        private void EventsReader_OnAttackStarted()
        {
            /*canExit = false;
            weaponProvider.Weapon.InvokeOnAttackStart();*/
        }

        private void EventsReader_OnAttackReleased()
        {
           Shoot();
        }

        private void EventsReader_OnAttackEnded()
        {
            /*EndAttack();
            isAttackInProgress = false;
            canExit = true;*/
            _animatorProvider.StopAttack();
        }
        
        #region Attack

        public void Attack(IInteractionData interactionData, IDamageData damageData)
        {
            SaveAttackTime();
            SpawnProjectile(interactionData, damageData);
        }
        

        protected void SpawnProjectile(IInteractionData interactionData, IDamageData damageData)
        {
            projectilesFactory.Create(damageData, OnProjectileCreated);

            void OnProjectileCreated(List<IProjectile> projectiles)
            {
                foreach (var projectile in projectiles)
                    projectile.OnSpawn(interactionData);
            }
        }
        
        public override void SetupProjectilesSpawner(ProjectilesSpawnHelper projectilesSpawnHelper) =>
            projectilesFactory = new ProjectilesFactory(_poolService,_ownerCharacter.StatsProvider, _projectilePrefab, projectilesSpawnHelper);
        
        private void SaveAttackTime() =>
            lastAttackTime = Time.time;

        #endregion
        
}
