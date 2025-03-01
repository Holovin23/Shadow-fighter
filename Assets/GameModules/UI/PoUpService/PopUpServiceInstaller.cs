using Zenject;

namespace TFPlay.Modules.UI.PopUpService
{
    public class PopUpServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PopUpService>().AsSingle();
        }
    }
}