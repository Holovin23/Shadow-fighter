using Pooling;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    public class EnemyFactory
    {
        
        private IColliderService _colliderService;
        
        private float radius = 15;
        private int mapSize = 100;
        private IPoolService _poolService;
        private SpawnPoints _spawnPoints;
        private Vector3 _spawnPositionSquare;
        private float _spawnRadius;

        public EnemyFactory(IPoolService poolService, IColliderService colliderService)
        {
            _colliderService = colliderService;
            _poolService = poolService;
        }
        
        public void SpawnEnemy(Enemy prefab,int Count, SpawnEnemiesWaveType waveType)
        {
            for (int i = 0; i < Count; i++)
            {
                var enemy = _poolService.Spawn(prefab,GetRandomPoint(),quaternion.identity);
                enemy.Initialize(); 
            }
        }

        private Vector3 GetRandomSpawnPosition(SpawnEnemiesWaveType waveType)
        {
            return _spawnPoints.spawnPoints.GetRandom();
        }
        
        private Vector3 GetRandomPoint()
        {
            Vector3 randomPoint;
            int attempts = 0;
            do
            {
                float angle = Random.Range(0f, Mathf.PI * 2); // Случайный угол
                randomPoint = new Vector3(
                    _colliderService.Player.transform.position.x + Mathf.Cos(angle) * radius, 0,
                    _colliderService.Player.transform.position.z + Mathf.Sin(angle) * radius
                );
                attempts++;
            }
            while (!IsInsideMap(randomPoint) && attempts < 100); // Повторяем, если точка вне карты

            return randomPoint;
        }
        
        private bool IsInsideMap(Vector3 point)
        {
            return Mathf.Abs(point.x) <= mapSize && Mathf.Abs(point.z) <= mapSize;
        }
    }
}