using TFPlay.Modules.Core.Haptic;
using TFPlay.Modules.SaveLoadSystem;
using TFPlay.Modules.SaveLoadSystem.Data;
using UnityEngine;
using Zenject;

namespace TFPlay.UI.SettingsUI
{
    public class VibrationSetting : MonoBehaviour
    {
        [SerializeField] private SettingUI _settingUI;

        [Inject] private ISaveLoadSystem _saveLoadSystem;
        [Inject] private IHapticService _hapticService;

        public void Initialize()
        {
            var value = _saveLoadSystem.GetData<SettingsSaveData>(SaveDataIds.SETTINGS).VibrationEnabled;
            _settingUI.Initialize(value);

            _settingUI.OnChangeValue += OnChangeValue;
        }

        private void OnChangeValue(float value)
        {
            var boolValue = value > 0;

            _hapticService.Enable(boolValue);

            _saveLoadSystem.GetData<SettingsSaveData>(SaveDataIds.SETTINGS).VibrationEnabled = boolValue;
            _saveLoadSystem.Save<SettingsSaveData>(SaveDataIds.SETTINGS);
        }
    }
}