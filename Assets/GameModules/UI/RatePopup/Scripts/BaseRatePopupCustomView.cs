using UnityEngine;

namespace TFPlay.Features.RatePopup
{
    public abstract class BaseRatePopupCustomView : MonoBehaviour
    {
        public abstract void Initialize(IRatePopupProvider popupProvider);
        public abstract void Show();
        public abstract void Hide();
    }
}