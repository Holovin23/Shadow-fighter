using System;
using System.Collections.Generic;
using GameModules.Gameplay.Scripts.Characters.Damage;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class PlayerMeleeAttackController : AttackController
    {
        [SerializeField] private List<TriggerArea> _triggerAreas = new List<TriggerArea>();
        [SerializeField] private  List<StayColliderTrigger> _meleeClosestEnemiesDetector = new List<StayColliderTrigger>();
        [SerializeField] private float _attackDelay = 1f;

        private IAnimatorProvider _animator;
        private float lastAttackTime;
        private bool _isReadyAttack;
        private void Update()
        {
            if (Time.time - lastAttackTime >= _attackDelay && !_isReadyAttack)
            {
                _isReadyAttack = true;
                Debug.Log(_isReadyAttack);
            }
        }

        public override void Initialize(Character owner)
        {
            base.Initialize(owner);
            _animator = owner.AnimatorProvider;
            foreach (var enterTrigger in _meleeClosestEnemiesDetector)
            {
                enterTrigger.OnInteraction += EnemyInMeleeRangeAttackRange;
            }
        }

        public void AddTriggerArea(TriggerArea triggerArea)
        {
            _triggerAreas.Add(triggerArea);
        }

        public void AddStayColliderTrigger(StayColliderTrigger stayColliderTrigger)
        {
            _meleeClosestEnemiesDetector.Add(stayColliderTrigger);
            stayColliderTrigger.OnInteraction += EnemyInMeleeRangeAttackRange;
        }
        
        private void EnemyInMeleeRangeAttackRange(Collider obj)
        {
            if(!_isReadyAttack)
                return;
            
            Debug.Log("Attacked");
            lastAttackTime = Time.time;
            _isReadyAttack = false;
            
            _animator.StartAttack();
            
            foreach (var areas in _triggerAreas)
            {
                foreach (var collider in areas.Cast())
                {
                    _colliderService.GetCharacter(collider, out Character character);
                    if (character != null && !character.IsDead)
                        character.HandleDamage(new DamageData(_ownerCharacter.transform, 10));
                    Debug.Log(character.gameObject.name);
                }
            }
        }
    }
}