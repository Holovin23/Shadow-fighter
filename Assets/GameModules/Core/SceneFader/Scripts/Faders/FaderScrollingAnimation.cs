using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TFPlay.SceneFader
{
    public class FaderScrollingAnimation : BaseSceneFaderAnimation
    {
        [Header("Settings")]
        [SerializeField] private float fadeTime = 0.5f;
        [SerializeField] private Ease fadeEase;

        [Header("Objects")]
        [SerializeField] private Image fadeImage;
        [SerializeField] private Image loadingCircle;

        [Header("Other")]
        [SerializeField] private CanvasGroup loadingTextCanvas;
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private string loadingTextStr = "Loading";
        [SerializeField] private float dotsSpeed = 1;

        public override void Show(Action callback)
        {
            fadeImage.transform.DOKill();
            fadeImage.transform.position = Vector3.zero;
            loadingTextCanvas.DOFade(1f, fadeTime).SetEase(fadeEase);
            loadingCircle.DOFade(1f, fadeTime).SetEase(fadeEase);
            fadeImage.DOFade(1f, fadeTime).SetEase(fadeEase).OnComplete(() => callback?.Invoke()).OnUpdate(LoadingText);
            fadeImage.transform.DOMove(Vector3.left * fadeImage.preferredWidth, fadeTime * 20);
            loadingCircle.transform.DORotate(Vector3.back * fadeImage.preferredWidth, fadeTime * 20).SetEase(Ease.Linear);
        }
        public override void Hide(Action callback)
        {
            loadingTextCanvas.DOFade(0, fadeTime);
            loadingCircle.DOFade(0, fadeTime);
            fadeImage.DOFade(0f, fadeTime).SetEase(fadeEase).OnComplete(() => callback?.Invoke());
        }
        public override void ShowImmediately()
        {
            gameObject.SetActive();
            fadeImage.color = fadeImage.color.WithAlpha(1);
            loadingTextCanvas.alpha = 1;
            loadingText.text = string.Empty;
        }
        public override void HideImmediately()
        {
            fadeImage.color = fadeImage.color.WithAlpha(0);
            loadingTextCanvas.alpha = 0;
            loadingText.text = string.Empty;
        }

        private void LoadingText()
        {
            loadingText.text = loadingTextStr;
            for (int i = 0; i < Mathf.Floor((Time.time * dotsSpeed) % 4); i++)
                loadingText.text += ".";
        }
    }
}
