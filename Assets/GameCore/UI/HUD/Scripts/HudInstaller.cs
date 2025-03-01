using UnityEngine;
using Zenject;

namespace TFPlay.UI
{
    public class HudInstaller : MonoInstaller
    {
        [SerializeField] private Hud _hud;

        public override void InstallBindings()
        {
            Container.Bind<IHud>().FromInstance(_hud);
        }
    }
}