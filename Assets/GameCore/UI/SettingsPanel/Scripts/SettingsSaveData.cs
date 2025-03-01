using System;

namespace TFPlay.Modules.SaveLoadSystem.Data
{
    [Serializable]
    public class SettingsSaveData : RootSaveData
    {
        public float SoundVolume = 1f;
        public float MusicVolume = 1f;
        public bool VibrationEnabled = true;
    }
}