using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pooling
{
    public class Pool : MonoBehaviour
    {
        [SerializeField] private List<Component> _storedItems = new();
        [SerializeField] private List<Component> _spawnedItems = new();
        [SerializeField] private List<Component> _totalItems = new();
        
        private PoolConfiguration _poolConfiguration;
        private IPoolableItemFactory _factory;

        public void Construct(PoolConfiguration poolConfiguration, IPoolableItemFactory factory)
        {
            _factory = factory;
            _poolConfiguration = poolConfiguration;
        }

        public void Initialize()
        {
            for (int i = 0; i < _poolConfiguration.Preload; i++)
            {
                CreateNewItem();
            }
        }
        
        public T Spawn<T>(T prefab) where T : Component
        {
            T item = GetItemToSpawn<T>();
            RemoveItem(item);
            item.SetActive();
            NotifyOnSpawn(item);
            
            return item;
        }

        public void Despawn<T>(T item) where T : Component
        {
            if(!_spawnedItems.Contains(item))
                return;
                
            NotifyOnDespawn(item);
            AddItem(item);
        }

        public void DespawnDelayed<T>(T item, float delay) where T : Component => 
            this.DelayedInvoke(delay, () => Despawn(item));

        public bool HasItemsToSpawn() => _storedItems.Count > 0;

        private void NotifyOnSpawn(Component item)
        {
            if (item is IPoolable poolable) 
                poolable.OnSpawn();
        }

        private void NotifyOnDespawn(Component item)
        {
            if (item is IPoolable poolable) 
                poolable.OnDespawn();
        }

        private T GetItemToSpawn<T>() where T : Component
        {
            Component itemToSpawn = _storedItems.FirstOrDefault();
            
            if (itemToSpawn == null)
            {
                if(_poolConfiguration.Recycle && _totalItems.Count > 0)
                    itemToSpawn = RecycleItem();
                else
                    itemToSpawn = CreateNewItem();
            }

            return itemToSpawn as T;
        }

        private Component RecycleItem()
        {
            Component item = _spawnedItems.First();
            Despawn(item);
            return item;
        }

        private Component CreateNewItem()
        {
            var capacity = _poolConfiguration.Capacity;
            if (capacity != 0 && capacity <= _totalItems.Count)
                return null;
            
            Component item = _factory.Create(transform);
            AddItem(item);
            _totalItems.Add(item);
            return item;
        }
        
        private void AddItem(Component item)
        {
            _storedItems.Add(item);
            _spawnedItems.Remove(item);
            item.transform.SetParent(transform);
            item.SetInactive();
        }

        private void RemoveItem(Component item)
        {
            _storedItems.Remove(item);
            _spawnedItems.Add(item);
            item.transform.SetParent(null);
            item.SetActive();
        }
    }
}