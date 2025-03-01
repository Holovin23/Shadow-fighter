using System;

namespace TFPlay.UI
{
    public interface IHud
    {
        public void Initialize();
        public void Show(Action onEnd = null);
        public void Hide(Action onEnd = null);
    }
}