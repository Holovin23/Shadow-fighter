using Zenject;

namespace TFPlay.Core.Input
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputBasicService>().To<InputBasicService>().AsSingle().NonLazy();
            Container.Bind<IInputDragService>().To<InputDragService>().AsSingle().NonLazy();
            Container.Bind<ISwipeDirectionsService>().To<Swipe4DirectionsService>().AsSingle();
            Container.Bind<IInputSwipeService>().To<InputSwipeService>().AsSingle().NonLazy();
        }
    }
}