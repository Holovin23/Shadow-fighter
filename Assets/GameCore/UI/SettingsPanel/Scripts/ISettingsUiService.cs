using System;

namespace TFPlay.UI.SettingsUI
{
    public interface ISettingsUiService
    {
        public void Initialize();
        public void Show(Action onEnd = null);
        public void Hide(Action onEnd = null);
    }
}