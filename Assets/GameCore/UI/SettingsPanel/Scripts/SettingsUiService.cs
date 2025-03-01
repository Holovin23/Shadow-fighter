using UnityEngine;

namespace TFPlay.UI.SettingsUI
{
    public class SettingsUiService : BaseUI, ISettingsUiService
    {
        [SerializeField] private MusicSettings _musicSettings;
        [SerializeField] private SoundSettings _soundSettings;
        [SerializeField] private VibrationSetting _vibrationSetting;
        [SerializeField] private GraphicsSettings _graphicsSettings;

        public override void Initialize()
        {
            base.Initialize();
            
            _musicSettings.Initialize();
            _soundSettings.Initialize();
            _vibrationSetting.Initialize();
            _graphicsSettings.Initialize();
        }
    }
}