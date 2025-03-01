using Zenject;

namespace TFPlay.Modules.GameResources
{
    public class GameResourcesServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameResourcesService>().To<GameResourcesService>().AsSingle();
            Container.DeclareSignal<ResourceUpdateSignal>();
            Container.DeclareSignal<AllResourcesUpdateSignal>();
        }
    }
}