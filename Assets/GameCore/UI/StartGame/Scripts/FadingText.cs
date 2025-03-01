using DG.Tweening;
using TMPro;
using UnityEngine;

namespace TFPlay.UI
{
    public class FadingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private Ease _ease = Ease.Linear;

        private Tween _tween;

        private void OnEnable()
        {
            StartAnimation();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void StartAnimation()
        {
            _text.alpha = 0f;
            _text.DOFade(1f, _duration).SetLoops(-1, LoopType.Yoyo).SetEase(_ease);
        }

        private void StopAnimation()
        {
            _tween?.Kill();
        }
    }
}