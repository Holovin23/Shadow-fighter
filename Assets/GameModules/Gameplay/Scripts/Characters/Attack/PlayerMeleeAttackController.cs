using System;
using System.Collections.Generic;
using GameModules.Gameplay.Scripts.Characters.AnimatorsScripts;
using GameModules.Gameplay.Scripts.Characters.Damage;
using TFPlay.SceneFader;
using UnityEngine;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class PlayerMeleeAttackController : AttackController
    {
        [SerializeField] private List<TriggerCaster> _triggerAreas = new List<TriggerCaster>();
        [SerializeField] private  List<StayColliderTrigger> _meleeClosestEnemiesDetector = new List<StayColliderTrigger>();

        private IAnimatorProvider _animatorProvider;
        private IAnimatorReader _animatorReader;
        private float lastAttackTime;
        private bool _isReadyAttack;
        private void Update()
        {
            if (Time.time - lastAttackTime >= _attackDelay && !_isReadyAttack)
            {
                _isReadyAttack = true;
                lastAttackTime = Time.time;
                _isReadyAttack = false;

                _animatorProvider.StartAttack();
            }
        }

        public override void Initialize(Character owner)
        {
            base.Initialize(owner);
            _animatorProvider = owner.AnimatorProvider;
            _animatorReader = owner.AnimationEventsReader;
            
            _animatorReader.OnAttackStarted += EventsReader_OnAttackStarted;
            _animatorReader.OnAttackReleased += EventsReader_OnAttackReleased;
            _animatorReader.OnAttackEnded += EventsReader_OnAttackEnded;
            
            foreach (var enterTrigger in _meleeClosestEnemiesDetector)
            {
                enterTrigger.OnInteraction += EnemyInMeleeRangeAttackRange;
            }
        }

        public void AddTriggerArea(TriggerCaster triggerCaster)
        {
            _triggerAreas.Add(triggerCaster);
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
            
            lastAttackTime = Time.time;
            _isReadyAttack = false;

            _animatorProvider.StartAttack();
        }

        private void Attack()
        {
            foreach (var areas in _triggerAreas)
            {
                foreach (var collider in areas.Cast())
                {
                    _colliderService.GetCharacter(collider, out Character character);
                    if (character != null && !character.IsDead)
                        character.Damageable.HandleDamage(new DamageData(_ownerCharacter.transform, 10));
                }
            }
        }
        
        private void EventsReader_OnAttackStarted()
        {
            /*canExit = false;
            weaponProvider.Weapon.InvokeOnAttackStart();*/
        }

        private void EventsReader_OnAttackReleased()
        {
            Attack();
        }

        private void EventsReader_OnAttackEnded()
        {
            /*EndAttack();
            isAttackInProgress = false;
            canExit = true;*/
            _animatorProvider.StopAttack();
        }

        private void OnDestroy()
        {
            _animatorReader.OnAttackStarted -= EventsReader_OnAttackStarted;
            _animatorReader.OnAttackReleased -= EventsReader_OnAttackReleased;
            _animatorReader.OnAttackEnded -= EventsReader_OnAttackEnded;
        }
    }
}