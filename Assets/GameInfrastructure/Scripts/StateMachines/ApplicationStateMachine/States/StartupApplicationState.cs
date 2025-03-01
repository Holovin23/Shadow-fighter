using TFPlay.Modules.Levels;

namespace TFPlay.Infrastructure.StateMachine.Application
{
    public class StartupApplicationState : IState
    {
        private ILevelsService _progressService;
        public StartupApplicationState(ILevelsService progressService)
        {
            _progressService = progressService;
        }

        public void Enter()
        {
            _progressService.Restart();
        }

        public void Exit()
        {
        }
    }
}