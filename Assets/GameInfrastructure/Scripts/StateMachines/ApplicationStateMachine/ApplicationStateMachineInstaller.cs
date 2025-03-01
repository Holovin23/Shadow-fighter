using TFPlay.Infrastructure.StateMachine.Application;
using Zenject;

namespace TFPlay.Infrastructure.Application
{
    public class ApplicationStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ApplicationStatesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ApplicationStateMachine>().AsSingle();
        }
    }
}