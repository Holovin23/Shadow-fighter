using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace NewUI.Animations
{
    [System.Serializable]
    public class FadeAnimationSettings
    {
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private float _toFade = 1f;
        [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);

        public Tween DoFade(MaskableGraphic fadableObject)
        {
            return fadableObject.DOFade(_toFade, _duration).From(fadableObject.color.a).SetEase(_curve)
                .SetLink(fadableObject.gameObject);
        }

        public Tween DoFade(CanvasGroup fadableObject)
        {
            return fadableObject.DOFade(_toFade, _duration).From(fadableObject.alpha).SetEase(_curve)
                .SetLink(fadableObject.gameObject);
        }
    }
}