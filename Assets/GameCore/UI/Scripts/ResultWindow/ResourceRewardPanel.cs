using TFPlay.Modules.GameResources;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace NewUI
{
    public class ResourceRewardPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private Image _image;
        public void Init(ResourceConfigData data, int count)
        {
            _image.sprite = data.Icon;
            _amountText.text = count.ToString();
        }
    }
}