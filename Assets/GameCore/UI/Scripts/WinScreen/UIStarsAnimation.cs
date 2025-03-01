using DG.Tweening;
using UnityEngine;

namespace NewUI
{
    [System.Serializable]
    public class UIStarsAnimation
    {
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private AnimationCurve _moveCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private AnimationCurve _scaleCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [SerializeField] private StarsData[] _animationsData;

        private Sequence _currentAnimation;

        public int StartsCount { get { return _animationsData.Length; } }

        public Sequence StartAnimation(int startsCount)
        {
            PrepairAnimations();
            return CreateAnimation(startsCount);
        }
        private Sequence CreateAnimation(int startsCount)
        {
            _currentAnimation = DOTween.Sequence();
            for (int i = 0; i < startsCount && i < _animationsData.Length; i++)
            {
                _currentAnimation.Append(_animationsData[i].SetAnimation(_duration, _moveCurve,_scaleCurve));
            }
            return _currentAnimation;
        }
        private void PrepairAnimations()
        {
            _currentAnimation?.Kill();
            foreach (var animation in _animationsData)
            {
                animation.PrepairAnimation();
            }
        }

        #region(EDITOR)
        public void Validate()
        {
            foreach (var animation in _animationsData)
            {
                animation?.Validate();
            }
        }
        #endregion


        [System.Serializable]
        private class StarsData
        {
            [Space(10)]
            [SerializeField] private RectTransform _target;
            [Header("Start")]
            [SerializeField] private Vector3 _startLocalPosition;
            [SerializeField] private Vector3 _startLocalScale = Vector3.zero;
            [Header("End")]
            [SerializeField] private Vector3 _endLocalPosition;
            [SerializeField] private Vector3 _endLocalScale = Vector3.one;
            public Tween SetAnimation(float duration, AnimationCurve moveCurve, AnimationCurve scaleCurve)
            {
                Sequence result = DOTween.Sequence();
                result.Append(_target.DOAnchorPos(_endLocalPosition, duration).SetEase(moveCurve).From(_startLocalPosition))
                    .Join(_target.DOScale(_endLocalScale, duration).SetEase(scaleCurve).From(_startLocalScale))
                    .OnStart(() =>
                    {
                        _target.SetActive();
                    });
                return result;
                    ;
            }
            public void PrepairAnimation()
            {
                _target.gameObject.SetActive(false);
            }
            public void Validate()
            {
                if (_target != null)
                {
                    _endLocalPosition = _target.anchoredPosition;
                    _endLocalScale = _target.localScale;
                }
            }
        }
    }
}
