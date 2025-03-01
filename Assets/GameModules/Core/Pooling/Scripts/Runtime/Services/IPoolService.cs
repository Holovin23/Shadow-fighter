using UnityEngine;

namespace Pooling
{
    public interface IPoolService
    {
        void Initialize();
        T Spawn<T>(T prefab) where T : Component;
        T Spawn<T>(T prefab, Transform parent) where T : Component;
        T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component;
        void Despawn<T>(T item, float delay = 0) where T : Component;
        T SpawnAndDespawnInTime<T>(T prefab, float timeToDespawn, Vector3 position, Quaternion rotation, Transform parent = null) where T : Component;
        bool HasItemsToSpawn<T>(T item) where T : Component;
    }
}