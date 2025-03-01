using Cysharp.Threading.Tasks;

namespace TFPlay.Infrastructure.StateMachine.Application
{
    public class LoadingSceneApplicationState : IPayloadState<int>
    {
        private ApplicationStateMachine _applicationStateMachine;
        private SceneLoader _sceneLoader;

        public LoadingSceneApplicationState(ApplicationStateMachine applicationStateMachine,
            SceneLoader sceneLoader)
        {
            _applicationStateMachine = applicationStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(int data)
        {
            LoadScene(data);
        }

        public void Exit()
        {

        }

        private async UniTask LoadScene(int sceneId)
        {
            await _sceneLoader.LoadScene(sceneId);

            _applicationStateMachine.Enter<GameApplicationState>();
        }
    }
}