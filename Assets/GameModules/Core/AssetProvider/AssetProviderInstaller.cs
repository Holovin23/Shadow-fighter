using Zenject;

namespace TFPlay.Modules.Core.AssetProvider
{
    public class AssetProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAssetProvider>().To<ResourcesAssetProvider>().AsSingle();
        }
    }
}