using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class QAHideCanvasContent : QAHideContentBase
{
    [SerializeField] private Canvas canvas;

    private void OnValidate()
    {
        canvas = GetComponent<Canvas>();
    }

    public override void ToggleContent()
    {
        canvas.enabled = !canvas.enabled;
    }
}
