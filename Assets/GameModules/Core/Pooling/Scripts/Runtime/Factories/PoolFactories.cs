using Zenject;

namespace Pooling
{
    public class PoolFactories
    {
        private readonly DiContainer _diContainer;

        public PoolFactories(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public IPoolableItemFactory GetFactory(PoolConfiguration poolConfiguration)
        {
            DiContainer subContainer = _diContainer.CreateSubContainer();
            subContainer.Bind<PoolConfiguration>().FromInstance(poolConfiguration).AsSingle();
            return subContainer.Instantiate(TypeSerializer.Deserialize(poolConfiguration.FactoryTypeSerialized)) as IPoolableItemFactory;
        }
    }
}