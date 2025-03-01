using System.Collections.Generic;

namespace TFPlay.Modules.SaveLoadSystem
{
    public interface ISaveDataProvider
    {
        void SetData<TData>(TData data, string id) where TData : RootSaveData;
        object GetData(string id);
        Dictionary<string, RootSaveData> AllData { get; }
    }
}