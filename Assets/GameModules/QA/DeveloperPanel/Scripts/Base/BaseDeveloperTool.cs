using UnityEngine;
using UnityEngine.UI;

namespace TFPlay.DeveloperUtilities
{
    public abstract class BaseDeveloperTool : MonoBehaviour
    {
        [SerializeField] protected Transform content;
        [SerializeField] protected Canvas canvas;
        [SerializeField] protected Button toggleConsole;

        protected bool isOpened;

        private void Start()
        {
            toggleConsole.onClick.AddListener(TogglePanel);
            InitInternal();
        }

        public virtual void Show()
        {
            canvas.enabled = true;
        }

        public virtual void Hide()
        {
            canvas.enabled = false;
        }

        protected abstract void InitInternal();

        protected virtual void TogglePanel()
        {
            isOpened = !isOpened;
            content?.gameObject.SetActive(isOpened);
        }
    }
}
