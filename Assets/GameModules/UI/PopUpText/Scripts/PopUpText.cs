using DG.Tweening;
using UnityEngine;
using TMPro;

namespace TFPlay.Modules.PopUpText
{
    public class PopUpText : MonoBehaviour
    {
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] TMP_Text _messageLabel;
        [SerializeField] RectTransform _rect;
        [Space] 
        [SerializeField] private float lifetime = 1f;
        [SerializeField] private float fadeInTime = .25f;
        [SerializeField] private float fadeOutTime = .5f;
        [SerializeField] private Vector3 endPointOffset = new(0f, 300f, 0f);
        [SerializeField] private Ease easing = Ease.OutCubic;

        public void Show(string message, Vector3 position, Vector2 size)
        {
            _rect.sizeDelta = size;

            Show(message, position);
        }

        public void Show(string message, Vector3 position)
        {
            _messageLabel.text = message;
            transform.position = position;
            gameObject.SetActive(true);
            Animate(endPointOffset, fadeInTime, lifetime, fadeOutTime);
        }

        private void Animate(Vector3 endPointOffset, float fadeInTime, float lifetime, float fadeOutTime)
        {
            _canvasGroup.alpha = 0f;
            transform.DOMove(transform.position + endPointOffset, fadeInTime + lifetime + fadeOutTime);
            var animSequence = DOTween.Sequence();
            animSequence.Append(_canvasGroup.DOFade(1, fadeInTime).SetEase(easing));
            animSequence.AppendInterval(lifetime);
            animSequence.Append(_canvasGroup.DOFade(0, fadeOutTime));
            animSequence.OnKill(FinishMessage);
        }

        private void FinishMessage()
        {
            transform?.DOKill();
            _canvasGroup?.DOKill();

            gameObject.SetActive(false);
        }
    }
}