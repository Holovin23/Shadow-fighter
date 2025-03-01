using System;
using UnityEngine;

namespace TFPlay.Features.RatePopup
{
    public static class RatePopupController
    {
        private const string RatePopupShownKey = "RatePopupShown";

        public static event Action OnRatePopupShown;

        private static IRatePopupProvider ratePopupProvider;

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
#if UNITY_ANDROID
            ratePopupProvider = new AndroidCustomRatePopupProvider();
#elif UNITY_IOS
            ratePopupProvider = new iOSRatePoupProvider();
#endif
        }

        public static bool IsRatePopupShown => PlayerPrefs.GetInt(RatePopupShownKey) == 1;

        public static void ShowRatePopup()
        {
            if (ratePopupProvider == null)
                return;

            if (IsRatePopupShown)
                return;

            Debug.Log("RatePopup: Showing Rate Popup window...");
            ratePopupProvider.ShowRatePopup();

            PlayerPrefs.SetInt(RatePopupShownKey, 1);
            PlayerPrefs.Save();

            OnRatePopupShown?.Invoke();
        }
    }
}
