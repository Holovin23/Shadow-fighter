using System;
using DG.Tweening;
using GameModules.Gameplay.Scripts.Characters;
using GameModules.Gameplay.Scripts.GameLoop;
using Pooling;
using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.Lootable
{
    public class ExperienceDrop : Drop
    {
        [Inject] private IExperienceService _experienceService;
        [SerializeField] private TriggerArea _triggerArea;
        [SerializeField] private Transform _lootVisual;

        /*[Inject] // Zenject передаст IPoolService автоматически
        public void Construct(IPoolService poolService)
        {
            _poolService = poolService;
        }*/
        private void Start()
        {
            OnSpawn();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            _triggerArea.OnEnter += OnCollisionDetected;
        }

        private void OnDestroy()
        {
            _triggerArea.OnEnter -= OnCollisionDetected;
        }

        private void OnCollisionDetected(Collider obj)
        {
            _triggerArea.OnEnter -= OnCollisionDetected;
            _triggerArea.Disable();
            PlaySuckInAnimation(obj);
            _experienceService.AddExperience(_count);
            Debug.Log("Pick up expirience" + _count);
        }

        private void PlaySuckInAnimation(Collider collider)
        {
           _lootVisual.transform.DOMove(collider.transform.position, 0.5f).OnComplete(OnDespawn); 
        }
    }
}