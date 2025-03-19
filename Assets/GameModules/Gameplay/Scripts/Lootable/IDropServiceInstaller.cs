using UnityEngine;
using Zenject;

namespace GameModules.Gameplay.Scripts.Lootable
{
    public class IDropServiceInstaller : MonoInstaller
    {
        [SerializeField] private DropService _dropService;

        public override void InstallBindings()
        {
            Container.Bind<IDropService>().FromInstance(_dropService).AsSingle();
            Container.Bind<IInitializable>().To<DropService>().FromInstance(_dropService).AsSingle();
        }
    }
}