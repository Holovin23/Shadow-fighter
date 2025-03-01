using Zenject;

namespace TFPlay.Infrastructure.Application
{
    public class BootSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BootSceneInitializer>().AsSingle().NonLazy();
        }
    }
}