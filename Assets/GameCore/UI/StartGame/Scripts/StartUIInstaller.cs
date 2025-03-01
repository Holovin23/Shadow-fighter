using UnityEngine;
using Zenject;

namespace TFPlay.UI
{
    public class StartUIInstaller : MonoInstaller
    {
        [SerializeField] private StartUI _startUI;

        public override void InstallBindings()
        {
            Container.Bind<StartUI>().FromInstance(_startUI);
        }
    }
}