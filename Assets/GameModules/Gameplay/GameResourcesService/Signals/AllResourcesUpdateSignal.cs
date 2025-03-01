using System.Collections.Generic;

namespace TFPlay.Modules.GameResources
{
    public struct AllResourcesUpdateSignal
    {
        public Dictionary<ResourceType, int> ResourcesValues;
    }
}