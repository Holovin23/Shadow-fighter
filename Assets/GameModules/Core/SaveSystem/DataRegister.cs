using System.Collections.Generic;
using TFPlay.Modules.GameResources;

namespace TFPlay.Modules.SaveLoadSystem.Data
{
    public class DataRegister : BaseDataRegister
    {
        public override Dictionary<string, RootSaveData> RegisterData()
        {
            return new Dictionary<string, RootSaveData>
            {
                { SaveDataIds.SETTINGS, RegisterData<SettingsSaveData>() },
                { SaveDataIds.ADAPTIVE_PERFORMANCE, RegisterData<AdaptivePerformanceSaveData>() },
                { SaveDataIds.RESOURCES, RegisterData<ResourcesSaveData>() },
                { SaveDataIds.LEVELS, RegisterData<LevelsSaveData>() },
            };
        }
    }
}