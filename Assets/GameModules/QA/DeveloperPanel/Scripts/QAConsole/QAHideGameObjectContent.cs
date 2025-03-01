using UnityEngine;

[RequireComponent(typeof(Transform))]
public class QAHideGameObjectContent : QAHideContentBase
{
    [SerializeField] private Transform content;

    private void OnValidate()
    {
        content = GetComponent<Transform>();
    }

    public override void ToggleContent()
    {
        content.gameObject.SetActive(!content.gameObject.activeSelf);
    }
}
