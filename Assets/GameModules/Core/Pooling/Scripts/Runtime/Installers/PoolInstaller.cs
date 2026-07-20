using UnityEngine;
using Zenject;

namespace Pooling
{
    public class PoolInstaller : MonoInstaller
    {
        [SerializeField] private PoolService _poolService;
        
        public override void InstallBindings()
        {
            Container.Bind<PoolFactories>().AsSingle();
            Container.Bind<IPoolService>().FromInstance(_poolService).AsSingle();
        }
    }
}