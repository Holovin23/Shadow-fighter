using System;
using UnityEngine;

namespace TFPlay.SceneFader
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneFaderController : MonoBehaviour
    {
        [SerializeField] private BaseSceneFaderAnimation iosFader;
        [SerializeField] private BaseSceneFaderAnimation androidFader;

        public bool Shown { private set; get; }
        
        private ISceneFaderAnimation fader;
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();

#if UNITY_ANDROID
            fader = androidFader.GetComponent<ISceneFaderAnimation>();
#elif UNITY_IOS
            fader = iosFader.GetComponent<ISceneFaderAnimation>();
#else
            fader = GetComponentInChildren<ISceneFaderAnimation>(true);
#endif
        }

        private void Start()
        {
            ShowImmediately();
        }

        public void Show(Action callback = null)
        {
            if(Shown) return;
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.Show(() =>
            {
                callback?.Invoke();
                Shown = true;
            });
            canvasGroup.blocksRaycasts = true;
        }

        public void Hide(Action callback = null)
        {
            if(!Shown) return;
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.Hide(() =>
            {
                canvasGroup.blocksRaycasts = false;
                callback?.Invoke();
                Shown = false;
            });
        }

        public void ShowImmediately()
        {
            if(Shown) return;
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.ShowImmediately();
            canvasGroup.blocksRaycasts = true;
            Shown = true;
        }

        public void HideImmediately()
        {
            if(!Shown) return;
            if (fader == null) return;
            if (canvasGroup == null) return;

            fader.HideImmediately();
            canvasGroup.blocksRaycasts = false;
            Shown = false;
        }
    }
}
