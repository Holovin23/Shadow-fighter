using UnityEngine;

namespace TFPlay.SafeArea
{
    [ExecuteInEditMode]
    public class SafeAreaVisualizer : MonoBehaviour
    {
        [SerializeField] private RectTransform safeAreaPanel;
        [SerializeField] private RectTransform topPanel;
        [SerializeField] private RectTransform bottomPanel;

        private void Update()
        {
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            SafeArea.Refresh();

            topPanel.anchorMin = new Vector2(0f, SafeArea.AnchorMax.y);
            topPanel.anchorMax = Vector2.one;

            safeAreaPanel.anchorMin = SafeArea.AnchorMin;
            safeAreaPanel.anchorMax = SafeArea.AnchorMax;

            bottomPanel.anchorMin = Vector2.zero;
            bottomPanel.anchorMax = new Vector2(1f, SafeArea.AnchorMin.y);
        }
    }
}
