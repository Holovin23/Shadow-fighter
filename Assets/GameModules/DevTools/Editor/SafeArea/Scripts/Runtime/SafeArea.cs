using UnityEngine;

namespace TFPlay.SafeArea
{
    public static class SafeArea
    {
        private static Rect LastSafeArea = new Rect(0, 0, 0, 0);
        private static Vector2Int LastScreenSize = new Vector2Int(0, 0);
        private static ScreenOrientation LastOrientation = ScreenOrientation.Portrait;

        public static Vector2 AnchorMin { get; private set; }
        public static Vector2 AnchorMax { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Refresh()
        {
            var safeArea = Screen.safeArea;

            if (safeArea != LastSafeArea || Screen.width != LastScreenSize.x || Screen.height != LastScreenSize.y || Screen.orientation != LastOrientation)
            {
                LastScreenSize.x = Screen.width;
                LastScreenSize.y = Screen.height;
                LastOrientation = Screen.orientation;

                CalculateSafeArea(safeArea);
            }
        }

        private static void CalculateSafeArea(Rect r)
        {
            LastSafeArea = r;

            if (Screen.width > 0 && Screen.height > 0)
            {
                var anchorMin = r.position + Vector2.up * CalculateBannerHeight();
                var anchorMax = r.position + r.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                if (anchorMin.x >= 0 && anchorMin.y >= 0 && anchorMax.x >= 0 && anchorMax.y >= 0)
                {
                    AnchorMin = anchorMin;
                    AnchorMax = anchorMax;
                }
            }
        }

        private static int CalculateBannerHeight()
        {
            var dpiMultiplier = Mathf.RoundToInt(Screen.dpi / 160);
            if (Screen.height <= 400 * dpiMultiplier)
            {
                return 32 * dpiMultiplier;
            }
            else if (Screen.height <= 720 * dpiMultiplier)
            {
                return 64 * dpiMultiplier;
            }
            else if (Screen.height <= 896 * dpiMultiplier)
            {
                return 72 * dpiMultiplier;
            }
            else
            {
                return 96 * dpiMultiplier;
            }
        }
    }
}
