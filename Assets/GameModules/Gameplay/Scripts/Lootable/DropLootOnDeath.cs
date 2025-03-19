using System;
using GameModules.Gameplay.Scripts.Characters.Health;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace GameModules.Gameplay.Scripts.Lootable
{
    public class DropLootOnDeath : MonoBehaviour
    {
        [Inject] private IDropService _dropService;
        [SerializeField] private Health _health;
        [SerializeField] private int _dropValue;
        [SerializeField] private DropType _dropType;

        private void Start()
        {
            
            _health.OnDeathValue += Drop;
        }

        private void OnDestroy()
        {
            _health.OnDeathValue -= Drop;
        }

        private void Drop(IHealthProvider obj)
        {
            _dropService.SpawnDrop(_dropType, transform.position, _dropValue);
            _health.OnDeathValue -= Drop;
        }
    }
}