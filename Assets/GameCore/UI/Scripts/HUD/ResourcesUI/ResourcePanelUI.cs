using TFPlay.Modules.GameResources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanelUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button plusButton;
    [SerializeField] private Image plusButtonIcon;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private int _visibleItemsCount = 4;

    [Space]
    [SerializeField] private ResourcePanelAnimations animations;


    public void Init(ResourceConfigData data)
    {
        animations.Init(countText,canvasGroup);

        if (data == null) return;

        icon.sprite = data.Icon;
        plusButtonIcon.color = data.Color;
    }

    public void LoadResourceCount(int count)
    {
        countText.text = count.ToString();
        animations.UpdateCount(count);
    }
    public void LoadPlaceholder(int placeholderIndex)
    {
        float targetFade = GetFade(placeholderIndex);
        canvasGroup.alpha = targetFade;
        canvasGroup.transform.localPosition = Vector3.zero;
    }



    public void SetResourceCount(int count)
    {
        animations.StartTextAnimation(count);
    }
    public void SetPlaceholder(int placeholderIndex)
    {
        float targetFade = GetFade(placeholderIndex);
        animations.StartFadeAnimation(targetFade);
        animations.StartAdjustAnimation();
    }

    private float GetFade(int placeholderIndex)
    {
        if (placeholderIndex >= _visibleItemsCount)
        {
            return 0f;
        }
        return 1;
    }
    private void Start()
    {
        plusButton.onClick.AddListener(OnClickPlus);
    }

    private void OnClickPlus()
    {
        //RV logic place
    }
}