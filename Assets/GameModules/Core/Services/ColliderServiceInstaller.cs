using Zenject;

namespace GameModules.Core.Services
{
    public class ColliderServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IColliderService>().To<ColliderService>().AsSingle().NonLazy();
        }
    }
}