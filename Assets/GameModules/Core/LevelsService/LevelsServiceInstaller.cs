using GameModules.Gameplay.Scripts.GameLoop;
using TFPlay.SceneFader;
using UnityEngine;
using Zenject;

namespace TFPlay.Modules.Levels
{
    public class LevelsServiceInstaller : MonoInstaller
    {
        [SerializeField] private SceneFaderController sceneFaderController;
        [SerializeField] private AllLevelsData _allLevelsData;
        public override void InstallBindings()
        {
            Container.Bind<AllLevelsData>().FromInstance(_allLevelsData).AsSingle();
            Container.Bind<ILevelsService>().To<LevelsService>().AsSingle().NonLazy();
            Container.Bind<SceneLoader>().AsSingle().NonLazy();
            Container.Bind<SceneFaderController>().FromInstance(sceneFaderController).AsSingle();
        }
    }
}