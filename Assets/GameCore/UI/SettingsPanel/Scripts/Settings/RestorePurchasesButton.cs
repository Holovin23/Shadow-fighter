using TFPlay.Modules.Core.Haptic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace TFPlay.UI.SettingsUI
{
    public class RestorePurchasesButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [Inject] private IHapticService _hapticService;
        
        private void Start()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            _button.SetInactive();
            return;
#endif

            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _hapticService.TapUI();
            //call restore purchases here
        }
    }
}