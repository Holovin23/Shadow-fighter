using UnityEngine;
using Zenject;

namespace TFPlay.Modules.PopUpText
{
    public class PopUpTextInstaller : MonoInstaller
    {
        [SerializeField] private PopUpTextService _popUpTextService;

        public override void InstallBindings()
        {
            Container.Bind<IPopUpTextService>().FromInstance(_popUpTextService).AsSingle();
        }
    }
}