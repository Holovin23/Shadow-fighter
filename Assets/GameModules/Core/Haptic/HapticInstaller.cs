using Zenject;

namespace TFPlay.Modules.Core.Haptic
{
    public class HapticInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IHapticService>().To<TapticService>().AsSingle();
        }
    }
}