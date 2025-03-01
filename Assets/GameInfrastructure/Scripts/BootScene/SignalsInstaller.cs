using Zenject;

namespace TFPlay.Infrastructure.Application
{
    public class SignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}