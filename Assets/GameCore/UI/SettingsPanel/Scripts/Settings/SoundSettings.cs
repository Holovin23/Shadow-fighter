using TFPlay.Modules.SaveLoadSystem;
using TFPlay.Modules.SaveLoadSystem.Data;
using UnityEngine;
using Zenject;

namespace TFPlay.UI.SettingsUI
{
    public class SoundSettings : MonoBehaviour
    {
        [SerializeField] private SettingUI _settingUI;

        [Inject] private ISaveLoadSystem _saveLoadSystem;
        // Inject audio service

        public void Initialize()
        {
            var value = _saveLoadSystem.GetData<SettingsSaveData>(SaveDataIds.SETTINGS).SoundVolume;
            _settingUI.Initialize(value);

            _settingUI.OnChangeValue += OnChangeValue;
        }

        private void OnChangeValue(float value)
        {
            // Sound volume change here

            _saveLoadSystem.GetData<SettingsSaveData>(SaveDataIds.SETTINGS).SoundVolume = value;
            _saveLoadSystem.Save<SettingsSaveData>(SaveDataIds.SETTINGS);
        }
    }
}