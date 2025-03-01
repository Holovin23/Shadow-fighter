using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace TFPlay.SceneFader
{
    public class FaderUpToDownAnimation : BaseSceneFaderAnimation
    {
        [Header("Objects")]
        [SerializeField] private RectTransform upImage;
        [SerializeField] private RectTransform downImage;

        [Header("Settings")]
        [SerializeField] private float animationTime;

        private float upImageStartY;
        private float downImageStartY;
        private float upImageTargetY = 0;
        private float downImageTargetY = 0;

        private void Awake()
        {
            upImageStartY = upImage.anchoredPosition.y;
            downImageStartY = downImage.anchoredPosition.y;
        }

        public override void Hide(Action callback)
        {
            StopAllCoroutines();
            StartCoroutine(HideAnimation(Help));
            void Help()
            {
                callback?.Invoke();
            }
        }
        public override void HideImmediately()
        {
            StopAllCoroutines();
            upImage.anchoredPosition = new Vector2(upImage.anchoredPosition.x, upImageStartY);
            downImage.anchoredPosition = new Vector2(downImage.anchoredPosition.x, downImageStartY);
            downImage.SetInactive();
        }

        public override void Show(Action callback)
        {
            StopAllCoroutines();
            StartCoroutine(ShowAnimation(Help));
            void Help()
            {
                callback?.Invoke();
            }
        }
        public override void ShowImmediately()
        {
            gameObject.SetActive();
            StopAllCoroutines();
            upImage.anchoredPosition = new Vector2(upImage.anchoredPosition.x, upImageStartY);
            downImage.anchoredPosition = new Vector2(downImage.anchoredPosition.x, downImageStartY);
            upImage.SetInactive();
        }

        private IEnumerator ShowAnimation(Action callback)
        {
            upImage.DOAnchorPosY(upImageStartY, animationTime / 2);

            yield return new WaitForSeconds(animationTime / 2);
            downImage.anchoredPosition = new Vector2(downImage.anchoredPosition.x, downImageStartY);
            upImage.SetInactive();
            yield return null;
            callback?.Invoke();
        }
        private IEnumerator HideAnimation(Action callback)
        {
            downImage.DOAnchorPosY(downImageTargetY, animationTime / 2);

            yield return new WaitForSeconds(animationTime / 2);
            upImage.anchoredPosition = new Vector2(upImage.anchoredPosition.x, upImageTargetY);
            upImage.SetActive();
            yield return null;
            callback?.Invoke();
        }
    }
}
