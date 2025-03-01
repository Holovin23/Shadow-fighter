using Zenject;

namespace TFPlay.Modules.Delays
{
    public class DelayServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DelayFactory>().AsSingle();
            Container.Bind<IDelayService>().To<DelayService>().AsSingle();
        }
    }
}