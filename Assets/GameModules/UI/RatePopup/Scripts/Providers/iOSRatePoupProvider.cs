#if UNITY_IOS
using UnityEngine.iOS;

namespace TFPlay.Features.RatePopup
{
    public class iOSRatePoupProvider : IRatePopupProvider
    {
        public void ShowRatePopup()
        {
            Device.RequestStoreReview();
        }
    }
}
#endif
