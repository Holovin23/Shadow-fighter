using UnityEngine;
using Zenject;

namespace Pooling
{
    public class PoolableItemFactory : IPoolableItemFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly PoolConfiguration _poolConfiguration;

        public PoolableItemFactory(PoolConfiguration poolConfiguration, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _poolConfiguration = poolConfiguration;
        }
        
        public Component Create(Transform parent)
        {
            Component item = _instantiator.InstantiatePrefab(_poolConfiguration.Prefab).GetComponent(_poolConfiguration.Prefab.GetType());
            item.transform.SetParent(parent);
            return item;
        }
    }
}