using UnityEngine;

namespace Pooling
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Pooling/PoolConfiguration")]
    public class PoolConfiguration : ScriptableObject
    {
        [SerializeField] private GameObject _prefab;
        
        public int Capacity;
        public int Preload;
        public bool Recycle;
        
        [HideInInspector] public Component Prefab;
        [HideInInspector] public string FactoryTypeSerialized;
        
        public PoolConfiguration Clone()
        {
            var clone = CreateInstance<PoolConfiguration>();
            clone._prefab = _prefab;
            clone.Prefab = Prefab;
            clone.Capacity = Capacity;
            clone.Preload = Preload;
            clone.Recycle = Recycle;
            clone.FactoryTypeSerialized = FactoryTypeSerialized;
            return clone;
        }
        
        public PoolConfiguration Clone<T>(GameObject prefab) where T : Component
        {
            var clone = CreateInstance<PoolConfiguration>();
            clone._prefab = prefab;
            clone.Prefab = prefab.GetComponent(typeof(T));
            clone.Capacity = Capacity;
            clone.Preload = Preload;
            clone.Recycle = Recycle;
            clone.FactoryTypeSerialized = FactoryTypeSerialized;
            return clone;
        }
    }
}