using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NewUI
{
    public class StarsUI : MonoBehaviour
    {
        [SerializeField] private UIStarsAnimation _starsAnimation;
        [SerializeField] private UIWinRaysAnimation _raysAnimation;
        private Sequence currentAnimation;

        public Sequence StartAnimation(int starsCount)
        {
            starsCount = Math.Clamp(starsCount,0,_starsAnimation.StartsCount);

            currentAnimation?.Kill();
            currentAnimation = DOTween.Sequence();

            currentAnimation.Append(_starsAnimation.StartAnimation(starsCount))
                .Append(_raysAnimation.StartAnimation(starsCount));
            return currentAnimation;
        }
        private void Update()
        {
            if (Keyboard.current[Key.A].wasPressedThisFrame)
            {
                StartAnimation(3);
            }
            if (Keyboard.current[Key.S].wasPressedThisFrame)
            {
                StartAnimation(2);
            }
            if (Keyboard.current[Key.D].wasPressedThisFrame)
            {
                StartAnimation(1);
            }
            if (Keyboard.current[Key.F].wasPressedThisFrame)
            {
                StartAnimation(5);
            }
            if (Keyboard.current[Key.G].wasPressedThisFrame)
            {
                StartAnimation(0);
            }
            if (Keyboard.current[Key.H].wasPressedThisFrame)
            {
                StartAnimation(-1);
            }
        }
        private void OnValidate()
        {
            _starsAnimation?.Validate();
        }
    }
}