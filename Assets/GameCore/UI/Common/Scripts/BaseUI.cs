using System;
using DG.Tweening;
using UnityEngine;

namespace TFPlay.UI
{
    public class BaseUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AnimationSettings _showAnimation;
        [SerializeField] private AnimationSettings _hideAnimation;

        [SerializeField] private bool _showOnStart;

        public UIState State { get; private set; } = UIState.Uninitialized;

        private Tween _transition;

        public virtual void Initialize()
        {
            if (_showOnStart)
                ForceShow();
            else
                ForceHide();
        }

        public void ForceShow()
        {
            ShowInstant();
            OnEndShow();
        }

        public void Show(Action onEnd = null)
        {
            if (State != UIState.Hidden)
                return;

            gameObject.SetActive();
            onEnd += OnEndShow;

            ShowAnimation(onEnd);
        }

        public void ForceHide()
        {
            HideInstant();
            OnEndHide();
        }

        public void Hide(Action onEnd = null)
        {
            if (State != UIState.Shown)
                return;

            onEnd += OnEndHide;

            HideAnimation(onEnd);
        }

        protected virtual void ShowAnimation(Action onEnd = null)
        {
            _transition.Kill(true);
            State = UIState.Transition;

            _canvasGroup.alpha = 0f;
            _transition = _canvasGroup.DOFade(1, (_showAnimation.Duration)).From(_canvasGroup.alpha)
                .SetEase(_showAnimation.Easing).SetLink(_canvasGroup.gameObject)
                .OnComplete(() => onEnd?.Invoke());
        }

        protected virtual void HideAnimation(Action onEnd = null)
        {
            _transition.Kill(true);
            State = UIState.Transition;

            _canvasGroup.alpha = 1f;
            _transition = _canvasGroup.DOFade(0, _hideAnimation.Duration).From(_canvasGroup.alpha)
                .SetEase(_hideAnimation.Easing).SetLink(_canvasGroup.gameObject)
                .OnComplete(() => onEnd?.Invoke());
        }

        protected virtual void ShowInstant()
        {
            _transition.Kill(true);
            _canvasGroup.alpha = 1f;
            gameObject.SetActive();
        }

        protected virtual void HideInstant()
        {
            _transition.Kill(true);
            _canvasGroup.alpha = 0f;
            gameObject.SetInactive();
        }

        protected virtual void OnEndShow()
        {
            State = UIState.Shown;
        }

        protected virtual void OnEndHide()
        {
            State = UIState.Hidden;
            gameObject.SetInactive();
        }
    }
}