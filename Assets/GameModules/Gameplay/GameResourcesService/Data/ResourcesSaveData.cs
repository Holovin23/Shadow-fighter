using System;
using TFPlay.Modules.SaveLoadSystem;

namespace TFPlay.Modules.GameResources
{
    [Serializable]
    public class ResourcesSaveData : RootSaveData
    {
        public SerializableDictionary<ResourceType, int> resources = null;
    }
}