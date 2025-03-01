using TFPlay.Modules.Core.Haptic;
using TFPlay.Modules.SaveLoadSystem;
using TFPlay.Modules.SaveLoadSystem.Data;
using UnityEngine;
using Zenject;

namespace TFPlay.UI.SettingsUI
{
    public class MusicSettings : MonoBehaviour
    {
        [SerializeField] private SettingUI _settingUI;

        [Inject] private ISaveLoadSystem _saveLoadSystem;
        // Inject audio service

        public void Initialize()
        {
            var value = _saveLoadSystem.GetData<SettingsSaveData>(SaveDataIds.SETTINGS).MusicVolume;
            _settingUI.Initialize(value);

            _settingUI.OnChangeValue += OnChangeValue;
        }

        private void OnChangeValue(float value)
        {
            // Music volume change here

            _saveLoadSystem.GetData<SettingsSaveData>(SaveDataIds.SETTINGS).MusicVolume = value;
            _saveLoadSystem.Save<SettingsSaveData>(SaveDataIds.SETTINGS);
        }
    }
}