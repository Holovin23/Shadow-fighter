using System.Collections.Generic;
using TFPlay.Modules.Core.AssetProvider;
using TFPlay.Modules.GameResources;
using UI.UIElements.Layouts;
using UnityEngine;
using Zenject;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private int ShownRecourcesCount = 4;
    [SerializeField] private ResourcePanelUI resourcePanelPrefab;
    [SerializeField] private DynamicLayoutUI _dynamicLayoutUI;

    [Inject] private IGameResourcesService _resourcesService;
    [Inject] private IAssetProvider _assetProvider;
    [Inject] private SignalBus _signalBus;

    private GameResourcesConfig _resourcesConfig;
    private readonly Dictionary<ResourceType, ResourcePanelUI> _resourcePanels = new();

    private void Start()
    {
        _resourcesConfig = _assetProvider.Load<GameResourcesConfig>(AssetPath.GAME_RESOURCES_CONFIG);

        AddStartingResources();

        _signalBus.Subscribe<ResourceUpdateSignal>(OnResourceChanged);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<ResourceUpdateSignal>(OnResourceChanged);
    }

    private void AddStartingResources()
    {
        var resourcesList = new List<ResourcePanelUI>();

        foreach (var pair in _resourcesService.GetAllResources())
        {
            var panel = CreatePanel(pair.Key);
            panel.LoadResourceCount(pair.Value);
            resourcesList.Add(panel);
        }

        _dynamicLayoutUI.AddElements(resourcesList);
    }

    private void OnResourceChanged(ResourceUpdateSignal signal)
    {
        ResourcePanelUI panel;

        if (_resourcePanels.ContainsKey(signal.ResourceType))
        {
            panel = _resourcePanels[signal.ResourceType];
            _dynamicLayoutUI.MoveToIndex(panel, 0);
        }
        else
        {
            panel = CreatePanel(signal.ResourceType);
            _dynamicLayoutUI.InsertElement(0, panel);
        }

        panel.SetResourceCount(signal.TotalAfterChange);
    }

    private ResourcePanelUI CreatePanel(ResourceType resourceType)
    {
        var panel = Instantiate(resourcePanelPrefab, transform);
        panel.Init(_resourcesConfig.GetResourceData(resourceType));
        _resourcePanels[resourceType] = panel;
        return panel;
    }
}