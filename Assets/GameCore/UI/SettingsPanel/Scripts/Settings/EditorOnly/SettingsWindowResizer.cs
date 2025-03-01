using UnityEngine;

public class SettingsWindowResizer : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("References")]
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RectTransform _settingsContainer;
    [SerializeField] private RectTransform _restorePurchases;
    [SerializeField] private Transform _layoutGroup;

    [Header("Offset values")]
    [SerializeField] private float _headerHeight;
    [SerializeField] private float _settingPanelHeight;
    [SerializeField] private float _restorePurchasesHeight;
    [SerializeField] private float _addedHeight;
    [SerializeField] private float _containerBottomOffsetDefault;
    [SerializeField] private float _containerBottomOffsetRestorePurchases;

    public void Resize()
    {
        ResizeMainWindow();
    }

    private void ResizeMainWindow()
    {
        var currentHeight = _rectTransform.rect.height;

        var childCount = 0;

        foreach (Transform child in _layoutGroup.transform)
            if (child.gameObject.activeSelf)
                childCount++;

        var purchasesOffset = _restorePurchases.gameObject.activeSelf ? _restorePurchasesHeight : 0;

        var height = _headerHeight + _settingPanelHeight * childCount + purchasesOffset + _addedHeight;

        var heightDifference = Mathf.Abs(currentHeight - height);

        if (heightDifference < 1) return;

        _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, height);
        ResizeContainer();
    }

    private void ResizeContainer()
    {
        var offset = _restorePurchases.gameObject.activeSelf
            ? _containerBottomOffsetRestorePurchases
            : _containerBottomOffsetDefault;

        _settingsContainer.offsetMin = new Vector3(_settingsContainer.offsetMin.x, offset);
    }
#endif
}