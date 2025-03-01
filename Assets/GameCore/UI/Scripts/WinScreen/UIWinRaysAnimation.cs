using DG.Tweening;
using UnityEngine;

namespace NewUI
{
    [System.Serializable]
    public class UIWinRaysAnimation
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private float _animationDuration = 3;
        [SerializeField] private float _scaleMultiplyPerStar = 1.15f;
        [SerializeField] private AnimationCurve _scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);
        private Sequence currentAnimation;
        public Sequence StartAnimation(int starsCount = 3)
        {
            currentAnimation?.Kill();
            currentAnimation = DOTween.Sequence();

            currentAnimation//.Append(target.DOLocalRotate(new Vector3(0, 0, 360), animationDuration, RotateMode.FastBeyond360).From(Vector3.zero))
                            .Join(_target.DOScale(GetAnimationScale(starsCount), _animationDuration).SetEase(_scaleCurve).From(Vector3.one)).SetLoops(-1);
            return currentAnimation;
        }
        private Vector3 GetAnimationScale(int starsCount = 3)
        {
            return Vector3.one * Mathf.Pow(_scaleMultiplyPerStar, starsCount);
        }
    }
}