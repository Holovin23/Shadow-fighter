using TFPlay.Modules.GameResources;
using TMPro;
using UnityEngine;
using Zenject;

namespace TFPlay.UI
{
    public class CoinsUI
    {
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private ResourceType resource;

        [Inject] private IGameResourcesService _gameResourcesService;
        [Inject] private SignalBus _signalBus;

        protected void Init()
        {
            SetCoinsText(_gameResourcesService.GetCount(resource));

            _signalBus.Subscribe<ResourceUpdateSignal>(OnResourceChange);
            _signalBus.Subscribe<AllResourcesUpdateSignal>(OnAllResourcesChanged);
        }

        private void OnResourceChange(ResourceUpdateSignal signal)
        {
            if (signal.ResourceType == resource)
                SetCoinsText(_gameResourcesService.GetCount(resource));
        }

        private void OnAllResourcesChanged(AllResourcesUpdateSignal signal)
        {
            if (!signal.ResourcesValues.ContainsKey(resource))
                return;

            SetCoinsText(signal.ResourcesValues[resource]);
        }

        private void SetCoinsText(int value)
        {
            coinsText.text = value.ToString();
        }
    }
}