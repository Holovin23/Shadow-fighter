using TFPlay.UI.SettingsUI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TFPlay.UI
{
    public class SettingsButton : BaseButton
    {
        [SerializeField] private Button _button;

        [Inject] private ISettingsUiService _settingsUi;

        public override void Initialize()
        {
            _button.onClick.AddListener(OpenSettings);
        }

        private void OpenSettings()
        {
            _settingsUi.Show();
        }
    }
}