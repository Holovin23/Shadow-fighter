using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;

namespace TFPlay.SceneFader
{
    public class FaderCircleAnimation : BaseSceneFaderAnimation
    {
        [System.Serializable]
        public class AnimationData
        {
            public Image image;
            public float showScaleSize;
            public float hideScaleSize;
            public float showScaleDuration;
            public float hideScaleDuration;
            public float showFadeDuration;
            public float hideFadeDuration;
            public bool showFadeOut;
            public Ease scaleEase;
            public Ease fadeEase;
        }

        [SerializeField] private AnimationData[] animationItems;
        [SerializeField] private Image logo;

        private Sequence animationSequence;

        public override void Show(Action callback)
        {
            animationSequence?.Kill();
            animationSequence = DOTween.Sequence();
            for (int i = 0; i < animationItems.Length; i++)
            {
                var item = animationItems[i];
                var image = item.image;
                animationSequence.Join(image.transform.DOScale(item.showScaleSize, item.showScaleDuration).SetEase(item.scaleEase));
                if (item.showFadeOut)
                {
                    animationSequence.Join(image.DOFade(1f, item.showFadeDuration / 2f).SetEase(item.fadeEase).SetLoops(2, LoopType.Yoyo));
                }
                else
                {
                    animationSequence.Join(image.DOFade(1f, item.showFadeDuration).SetEase(item.fadeEase));
                }
            }
            animationSequence.OnComplete(() => callback?.Invoke());
        }

        public override void Hide(Action callback)
        {
            animationSequence?.Kill();
            animationSequence = DOTween.Sequence();
            for (int i = 0; i < animationItems.Length; i++)
            {
                var item = animationItems[i];
                var image = item.image;
                animationSequence.Join(image.transform.DOScale(item.hideScaleSize, item.hideScaleDuration).From(item.showScaleSize).SetEase(item.scaleEase));
                animationSequence.Join(image.DOFade(0f, item.hideFadeDuration).SetEase(item.fadeEase));
            }
            animationSequence.OnComplete(() =>
            {
                callback?.Invoke();
                HideImmediatelyInternal();
            });
        }

        public override void ShowImmediately()
        {
            gameObject.SetActive();
            ShowImmediatelyInternal();
        }

        public override void HideImmediately()
        {
            gameObject.SetInactive();
            HideImmediatelyInternal();
        }

        private void ShowImmediatelyInternal()
        {
            for (int i = 0; i < animationItems.Length; i++)
            {
                var item = animationItems[i];
                var image = item.image;
                image.transform.localScale = Vector3.one * item.showScaleSize;
                image.color = image.color.WithAlpha(1f);
            }
            logo.transform.localScale = Vector3.zero;
        }

        private void HideImmediatelyInternal()
        {
            for (int i = 0; i < animationItems.Length; i++)
            {
                var item = animationItems[i];
                var image = item.image;
                image.transform.localScale = Vector3.zero;
                image.color = image.color.WithAlpha(0f);
            }
        }
    }
}
