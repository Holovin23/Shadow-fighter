using System.Collections;
using GameModules.Gameplay.Scripts.Characters.AnimatorsScripts;
using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class RangedPlayerAttackController : AttackController
    {
        [Inject] private IColliderService _colliderService;
        [SerializeField] private float _attackDelay = 1f;
        [SerializeField] private float _searchEnemyRadius = 6f;
        [SerializeField] private LayerMask _enemyLayer;
        
        private IAnimatorProvider _animatorProvider;
        private IAnimatorReader _animatorReader;
        private IMovementProvider _movementProvider;
        private float lastAttackTime;
        private bool _isReadyAttack;
        private Character _currentTarget;
        private Coroutine _updateRoutine;
        private Collider[] _resultArray = new Collider[100];
        
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
            Debug.Log("Shootings to ");
           // _currentTarget
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
        
    }
}