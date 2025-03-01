#if UNITY_ANDROID
using UnityEngine;

namespace TFPlay.Features.RatePopup
{
    public class AndroidRatePopupProvider : IRatePopupProvider
    {
        private static readonly AndroidJavaClass RatePopupJavaWrapper = new AndroidJavaClass("com.ratepopup.wrapper.RatePopupJavaWrapper");

        public void ShowRatePopup()
        {
            RatePopupJavaWrapper.CallStatic("showRatePopup");
        }
    }
}
#endif