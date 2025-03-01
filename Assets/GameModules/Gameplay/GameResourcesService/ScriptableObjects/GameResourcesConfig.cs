using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TFPlay.Modules.GameResources
{
    [CreateAssetMenu(fileName = "GameResourcesConfig", menuName = "24Play/Configs/GameResourcesConfig", order = 1)]
    public class GameResourcesConfig : ScriptableObject
    {
        public List<ResourceConfigData> resourceConfigData;

        public ResourceConfigData GetResourceData(ResourceType resourceType)
        {
            return resourceConfigData.FirstOrDefault(r => r.Type == resourceType);
        }
    }
}