using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TFPlay.Modules.GameResources;

namespace NewUI
{
    public class ResultWindow : MonoBehaviour
    {
        [SerializeField] private Button _getButton;
        [SerializeField] private RectTransform _rewardHolder;
        [SerializeField] private ResourceRewardPanel _resourceRewardPanel;

        private List<ResourceRewardPanel> _rewards = new List<ResourceRewardPanel>();
        
        public void SetReward(ResourceConfigData reward,int count)
        {
            var instance = InstantiateReward();
            instance.Init(reward, count);
            _rewards.Add(instance);
        }

        private void Awake()
        {
            Init();
        }
        private void OnDisable()
        {
            ClearResults();
        }
        private void Init()
        {
            _rewards = GetComponentsInChildren<ResourceRewardPanel>().ToList();
            ClearResults();
        }
        private void ClearResults()
        {
            for (int i = 0; i < _rewards.Count; i++)
            {
                DestroyReward(_rewards[i]);
            }
            _rewards.Clear();
        }
        private ResourceRewardPanel InstantiateReward()
        {
            //Move to object pool
            var result = Instantiate(_resourceRewardPanel, _rewardHolder);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rewardHolder);
            return result;
        }
        private void DestroyReward(ResourceRewardPanel reward)
        {
            //Move to object pool
            Destroy(reward.gameObject);
        }
    }
}