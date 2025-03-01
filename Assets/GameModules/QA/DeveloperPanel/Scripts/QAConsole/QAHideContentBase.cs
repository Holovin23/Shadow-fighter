using TFPlay.DeveloperUtilities;
using UnityEngine;

public abstract class QAHideContentBase : MonoBehaviour, IQAHideableContent
{
    private void Start()
    {
        QAConsole.RegisterContent(this);
    }

    private void OnDestroy()
    {
        QAConsole.UnregisterContent(this);
    }

    public abstract void ToggleContent();
}
