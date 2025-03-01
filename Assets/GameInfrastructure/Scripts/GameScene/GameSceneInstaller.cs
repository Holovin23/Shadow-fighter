using TFPlay.Infrastructure.StateMachine.Game;
using Zenject;

namespace TFPlay.Infrastructure.GameScene
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SetupGameplayStateMachine();
            
            Container.BindInterfacesAndSelfTo<GameSceneInitializer>().AsSingle().NonLazy();
        }

        private void SetupGameplayStateMachine()
        {
            Container.Bind<GameplayStatesFactory>().AsSingle();
            Container.Bind<GameplayStateMachine>().AsSingle();
        }
    }
}