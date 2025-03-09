using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Pooling
{
    public class PoolService : MonoBehaviour, IPoolService
    {
        [SerializeField] private PoolConfiguration _defaultPoolConfig;
        [SerializeField] private PoolConfiguration[] _poolConfigs;

        private Dictionary<GameObject, Pool> _pools = new();
        private Dictionary<GameObject, Pool> _links = new();

        private PoolFactories _factories;

        [Inject]
        public void Construct(PoolFactories factories)
        {
            _factories = factories;
        }
        
        public void Initialize()
        {
            for (int index = 0; index < _poolConfigs.Length; index++)
            {
                PoolConfiguration poolConfiguration = _poolConfigs[index];
                CreatePool(poolConfiguration);
            }
        }

        public T Spawn<T>(T prefab) where T : Component
        {
            Pool pool = GetPool(prefab);
            if (!pool.HasItemsToSpawn())
            {
                pool.CreateNewItem();
            }
            T item = pool.Spawn(prefab); 
            _links.Add(item.gameObject, pool);
            return item;
        }

        public T Spawn<T>(T prefab, Transform parent) where T : Component
        {
            T item = Spawn(prefab);
            item.transform.SetParent(parent, false);
            return item;
        }

        public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            T item = Spawn(prefab);
            item.transform.SetParent(parent);
            item.transform.SetPositionAndRotation(position, rotation);
            return item;
        }

        public T SpawnAndDespawnInTime<T>(T prefab, float timeToDespawn, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component
        {
            T item = Spawn(prefab, position, rotation, parent);
            Despawn(item, timeToDespawn);
            return item;
        }

        public void Despawn<T>(T item, float delay = 0) where T : Component
        {
            Pool pool = GetPoolFromLink(item.gameObject);
            _links.Remove(item.gameObject);

            if (delay > 0)
                pool.DespawnDelayed(item, delay);
            else
                pool.Despawn(item);
        }

        public bool HasItemsToSpawn<T>(T item) where T : Component
        {
            Pool pool = GetPool(item);
            return pool.HasItemsToSpawn();
        }

        private Pool GetPool<T>(T prefab) where T : Component
        {
            if (!_pools.TryGetValue(prefab.gameObject, out Pool pool))
                pool = CreatePool(GetPoolConfig(prefab));

            return pool;
        }
        
        private Pool GetPoolFromLink(GameObject clone)
        {
            return _links[clone];
        }

        private PoolConfiguration GetPoolConfig<T>(T prefab) where T : Component
        {
            PoolConfiguration config = _poolConfigs.FirstOrDefault(config => config.Prefab.gameObject == prefab.gameObject);

            if (config == null)
                config = _defaultPoolConfig.Clone<T>(prefab.gameObject);

            return config;
        }
        
        private Pool CreatePool(PoolConfiguration poolConfiguration)
        {
            Pool pool = new GameObject($"{poolConfiguration.Prefab.name} Pool").AddComponent<Pool>();
            pool.transform.SetParent(transform, false);
            pool.Construct(poolConfiguration, _factories.GetFactory(poolConfiguration));
            pool.Initialize();
            _pools.Add(poolConfiguration.Prefab.gameObject, pool);
            return pool;
        }
    }
}