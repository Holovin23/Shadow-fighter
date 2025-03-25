using Zenject;

namespace GameModules.Gameplay.Scripts.GameLoop
{
    public class ExperienceServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IExperienceService>().To<ExperienceService>().AsSingle().NonLazy();
        }
    }
}