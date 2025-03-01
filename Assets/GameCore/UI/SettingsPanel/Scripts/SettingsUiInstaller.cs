using UnityEngine;
using Zenject;

namespace TFPlay.UI.SettingsUI
{
    public class SettingsUiInstaller : MonoInstaller
    {
        [SerializeField] private SettingsUiService _settingsUi;
        public override void InstallBindings()
        {
            Container.Bind<ISettingsUiService>().FromInstance(_settingsUi);
        }
    }
}