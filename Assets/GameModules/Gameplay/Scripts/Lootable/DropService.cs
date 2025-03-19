using System.Collections.Generic;
using Pooling;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.Lootable
{
    public class DropService : MonoBehaviour, IDropService, IInitializable
    {
        [Inject] private IPoolService _poolService;
        [SerializeField] private ExperienceDrop _experienceDropPrefab;
        
        private Dictionary<DropType,Drop> _dropDictinary = new Dictionary<DropType,Drop>();
        public void Initialize()
        {
            _dropDictinary.Add(DropType.Experience,_experienceDropPrefab);
        }

        public void SpawnDrop(DropType dropType, Vector3 position,int count)
        {
            var drop = _poolService.Spawn(_dropDictinary[dropType], position,quaternion.identity);
            drop.SetCount(count);
            drop.SetOnDespawn(Despawn);

            void Despawn()
            {
                _poolService.Despawn(drop); 
            }
        }
        

    }

    public enum DropType
    {
        None = 0,
        Gold = 1,
        Experience = 2,
    }
}