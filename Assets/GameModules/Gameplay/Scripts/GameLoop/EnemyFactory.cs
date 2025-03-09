using Pooling;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    public class EnemyFactory
    {
        
        [Inject] private IColliderService _colliderService;
        
        private IPoolService _poolService;
        private SpawnPoints _spawnPoints;
        private Vector3 _spawnPositionSquare;
        private float _spawnRadius;

        public EnemyFactory(IPoolService poolService,SpawnPoints spawnPoints)
        {
            _spawnPoints = spawnPoints;
            _poolService = poolService;
        }
        
        public void SpawnEnemy(Enemy prefab,int Count, SpawnEnemiesWaveType waveType)
        {
            for (int i = 0; i < Count; i++)
            {
                var enemy = _poolService.Spawn(prefab,GetRandomSpawnPosition(waveType),quaternion.identity);
                enemy.Initialize(); 
            }
        }

        private Vector3 GetRandomSpawnPosition(SpawnEnemiesWaveType waveType)
        {
            return _spawnPoints.spawnPoints.GetRandom();
        }
    }
}