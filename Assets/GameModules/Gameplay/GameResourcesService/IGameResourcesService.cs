using System.Collections.Generic;

namespace TFPlay.Modules.GameResources
{
    public interface IGameResourcesService
    {
        public void Initialize();
        public int GetCount(ResourceType resourceType);
        public Dictionary<ResourceType, int> GetAllResources();
        public void AddResource(ResourceType resourceType, int count);
        public void ClearResources();
    }
}