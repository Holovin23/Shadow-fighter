using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace TFPlay.SceneFader
{
    public class FaderLogosScaleAnimation : BaseSceneFaderAnimation
    {
        [Header("Objects references")]
        [SerializeField] private List<Transform> items = new List<Transform>();

        [Header("Animation settings")]
        [SerializeField] private float itemAppearTime = 1f;
        [SerializeField] private float itemHideTime = 1f;
        [SerializeField] private float itemAnimationDelay = .1f;
        [SerializeField] private Ease itemAnimationEase = Ease.InOutCubic;

        public override void Show(Action callback)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateAllItemsScale(Vector3.one, itemAppearTime, callback));
        }

        public override void Hide(Action callback)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateAllItemsScale(Vector3.zero, itemHideTime, callback));
        }

        public override void ShowImmediately()
        {
            gameObject.SetActive();
            SetAllItemsScale(Vector3.one);
        }

        public override void HideImmediately()
        {
            SetAllItemsScale(Vector3.zero);
        }

        private void SetAllItemsScale(Vector3 targetScale)
        {
            foreach (var item in items.Shuffle())
            {
                item.DOKill();
                item.localScale = targetScale;
            }
        }

        private IEnumerator AnimateAllItemsScale(Vector3 targetScale, float time, Action callback)
        {
            foreach (var item in items)
            {
                yield return new WaitForSeconds(itemAnimationDelay);
                item.DOKill();
                item.DOScale(targetScale, time).SetEase(itemAnimationEase);
            }
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
}
