using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

namespace TFPlay.SceneFader
{
    public class FaderBlackScreenAnimation : BaseSceneFaderAnimation
    {
        [Header("Settings")]
        [SerializeField] private float fadeTime = 0.5f;
        [SerializeField] private Ease fadeEase;

        [Header("Objects")]
        [SerializeField] private CanvasGroup content;
        [SerializeField] private Image circle;

        [Header("Other")]
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private string loadingTextStr = "Loading";
        [SerializeField] private float dotsSpeed = 1;
        [SerializeField] private float circleRotateTime = 5f;

        private Tween rotateCircleTween;

        public override void Show(Action callback)
        {
            content.DOFade(1f, fadeTime)
                .SetEase(fadeEase)
                .OnUpdate(LoadingText)
                .OnComplete(() => callback?.Invoke());
            rotateCircleTween = circle.transform.DORotate(Vector3.back * 360f, circleRotateTime, RotateMode.FastBeyond360)
                .SetLoops(-1);
        }

        public override void Hide(Action callback)
        {
            content.DOFade(0f, fadeTime)
                .SetEase(fadeEase)
                .OnComplete(() => 
                {
                    rotateCircleTween.Kill();
                    callback?.Invoke();
                });
        }

        public override void ShowImmediately()
        {
            gameObject.SetActive();
            content.alpha = 1f;
        }

        public override void HideImmediately()
        {
            content.alpha = 0f;
        }

        private void LoadingText()
        {
            loadingText.text = loadingTextStr;
            for (int i = 0; i < Mathf.Floor((Time.time * dotsSpeed) % 4); i++)
                loadingText.text += ".";
        }
    }
}
