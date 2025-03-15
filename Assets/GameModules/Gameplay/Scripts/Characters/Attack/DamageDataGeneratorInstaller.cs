using Zenject;

namespace GameModules.Gameplay.Scripts.Characters
{
    public class DamageDataGeneratorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IDamageGenerator>().To<DamageDataGenerator>().AsSingle().NonLazy();
        }
    }
}