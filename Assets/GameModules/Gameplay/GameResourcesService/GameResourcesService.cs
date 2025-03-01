using System.Collections.Generic;
using TFPlay.Modules.SaveLoadSystem;
using Zenject;

namespace TFPlay.Modules.GameResources
{
    public class GameResourcesService : IGameResourcesService
    {
        private readonly ISaveLoadSystem _saveLoadSystem;
        private SignalBus _signalBus;

        public GameResourcesService(ISaveLoadSystem saveLoadSystem, SignalBus signalBus)
        {
            _saveLoadSystem = saveLoadSystem;
            _signalBus = signalBus;
        }

        private Dictionary<ResourceType, int> _resources = new();

        public void Initialize()
        {
            Load();
        }

        public int GetCount(ResourceType resourceType)
        {
            if (!_resources.ContainsKey(resourceType))
                return 0;

            return _resources[resourceType];
        }

        public Dictionary<ResourceType, int> GetAllResources()
        {
            return new Dictionary<ResourceType, int>(_resources);
        }

        public void AddResource(ResourceType resourceType, int count)
        {
            if (_resources.ContainsKey(resourceType))
                _resources[resourceType] += count;
            else
                _resources[resourceType] = count;

            Save();

            _signalBus.Fire(new ResourceUpdateSignal
                { ResourceType = resourceType, ChangeAmount = count, TotalAfterChange = _resources[resourceType] });
        }

        public void ClearResources()
        {
            _resources.Clear();

            _signalBus.Fire(new AllResourcesUpdateSignal { ResourcesValues = GetAllResources() });
        }

        private void Load()
        {
            var saveData = _saveLoadSystem.GetData<ResourcesSaveData>(SaveDataIds.RESOURCES);

            if (saveData.resources == null)
                return;

            _resources = saveData.resources.GetDictionary();

            _signalBus.Fire(new AllResourcesUpdateSignal { ResourcesValues = GetAllResources() });
        }

        private void Save()
        {
            _saveLoadSystem.GetData<ResourcesSaveData>(SaveDataIds.RESOURCES).resources =
                new SerializableDictionary<ResourceType, int>(_resources);

            _saveLoadSystem.Save<ResourcesSaveData>(SaveDataIds.RESOURCES);
        }
    }
}