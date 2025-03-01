using System.Collections.Generic;
using UnityEngine;

namespace TFPlay.Modules.SaveLoadSystem
{
    public class SaveDataProvider : ISaveDataProvider
    {
        public Dictionary<string, RootSaveData> AllData { get; } = new();

        public void SetData<TData>(TData data, string id) where TData : RootSaveData
        {
            AllData[id] = data;
        }

        public object GetData(string id)
        {
            if (AllData.TryGetValue(id, out var data))
                return data;
            
            Debug.LogWarning($"Data of type '{id}' not found in the provider.");
            return null;
        }
    }
}