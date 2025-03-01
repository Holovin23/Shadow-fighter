using UnityEngine;

namespace Pooling
{
    public interface IPoolableItemFactory
    {
        Component Create(Transform parent);
    }
}