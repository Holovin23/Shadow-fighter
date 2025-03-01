using System.Collections;
using UnityEngine;

namespace TFPlay.Features.RatePopup
{
    public class RatePopupAutoShower : MonoBehaviour
    {
        private const int RatePopupShowDelay = 180;

#if !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (!RatePopupController.IsRatePopupShown)
            {
                var go = new GameObject("[RatePopupAutoShower]");
                go.AddComponent<RatePopupAutoShower>();
                DontDestroyOnLoad(go);
            }
        }
#endif

        private void OnEnable()
        {
            RatePopupController.OnRatePopupShown += RatePopupController_OnRatePopupShown;
        }

        private void OnDisable()
        {
            RatePopupController.OnRatePopupShown -= RatePopupController_OnRatePopupShown;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(RatePopupShowDelay);
            RatePopupController.ShowRatePopup();
        }

        private void RatePopupController_OnRatePopupShown()
        {
            Destroy(gameObject);
        }
    }
}
