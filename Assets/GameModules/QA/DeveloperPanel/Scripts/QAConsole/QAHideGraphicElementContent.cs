using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class QAHideGraphicElementContent : QAHideContentBase
{
    [SerializeField] private MaskableGraphic maskableGraphic;

    private float defaultAlpha;

    private void OnValidate()
    {
        maskableGraphic = GetComponent<MaskableGraphic>();
    }

    private void Awake()
    {
        defaultAlpha = maskableGraphic.color.a;
    }

    public override void ToggleContent()
    {
        maskableGraphic.color = maskableGraphic.color.WithAlpha(maskableGraphic.color.a > 0f ? 0f : defaultAlpha);
    }
}
