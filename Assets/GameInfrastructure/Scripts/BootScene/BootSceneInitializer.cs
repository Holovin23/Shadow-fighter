using TFPlay.Modules.GameResources;
using TFPlay.Modules.Levels;
using TFPlay.Modules.AdaptivePerformance;
using TFPlay.Modules.SaveLoadSystem;
using TFPlay.Infrastructure.StateMachine.Application;
using TFPlay.Modules.Delays;
using Zenject;

namespace TFPlay.Infrastructure.Application
{
    public class BootSceneInitializer : IInitializable
    {
        private readonly ApplicationStateMachine _applicationStateMachine;
        private readonly ILevelsService _progressService;
        private readonly IGameResourcesService _gameResourcesService;
        private readonly ISaveLoadSystem _sls;
        private readonly IAdaptivePerformanceService _performanceService;
        private readonly IDelayService _delayService;

        public BootSceneInitializer(ApplicationStateMachine applicationStateMachine,
            ILevelsService progressService,
            ISaveLoadSystem sls,
            IGameResourcesService gameResourcesService,
            IAdaptivePerformanceService performanceService, IDelayService delayService)
        {
            _applicationStateMachine = applicationStateMachine;
            _progressService = progressService;
            _gameResourcesService = gameResourcesService;
            _sls = sls;
            _performanceService = performanceService;
            _delayService = delayService;
        }

        public void Initialize()
        {
            UnityEngine.Application.targetFrameRate = 60;

            _sls.Load();
            _delayService.Initialize();
            _gameResourcesService.Initialize();
            _progressService.Initialize();
            _performanceService.Initialize();

            _applicationStateMachine.Initialize();
            _applicationStateMachine.Enter<StartupApplicationState>();
        }
    }
}