using UnityEngine;
using Zenject;

namespace TFPlay.Modules.Core.TickService
{
    public class TickServiceInstaller : MonoInstaller
    {
        [SerializeField] private TickService _tickService;

        public override void InstallBindings()
        {
            Container.Bind<ITickService>().FromInstance(_tickService);
            
            Container.DeclareSignal<TickSignal>().OptionalSubscriber();
            Container.DeclareSignal<FixedTickSignal>().OptionalSubscriber();
        }
    }
}