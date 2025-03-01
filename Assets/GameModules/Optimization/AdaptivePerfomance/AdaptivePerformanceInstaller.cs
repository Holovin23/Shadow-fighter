using UnityEngine;
using Zenject;

namespace TFPlay.Modules.AdaptivePerformance
{
    public class AdaptivePerformanceInstaller : MonoInstaller
    {
        [SerializeField] private AdaptivePerformanceService _adaptivePerformance;

        public override void InstallBindings()
        {
            Container.Bind<IAdaptivePerformanceService>().FromInstance(_adaptivePerformance).AsSingle().NonLazy();
        }
    }
}