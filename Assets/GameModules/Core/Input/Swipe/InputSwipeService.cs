using System;
using UnityEngine;

namespace TFPlay.Core.Input
{
    public class InputSwipeService : IInputSwipeService
    {
        public event Action<SwipeData> OnSwipe;

        private IInputBasicService _inputBasic;
        private ISwipeDirectionsService _swipeDirectionsService;

        private Vector2 _startTouchPosition;
        private Vector2 _endTouchPosition;

        public InputSwipeService(IInputBasicService inputBasicService, ISwipeDirectionsService swipeDirectionsService)
        {
            _swipeDirectionsService = swipeDirectionsService;
            _inputBasic = inputBasicService;
            
            _inputBasic.OnTouch += OnTouch;
            _inputBasic.OnRelease += OnRelease;
        }

        ~InputSwipeService()
        {
            if (_inputBasic == null) return;

            _inputBasic.OnTouch -= OnTouch;
            _inputBasic.OnRelease -= OnRelease;
        }

        private void OnTouch()
        {
            _startTouchPosition = _inputBasic.GetTouchPosition();
        }

        private void OnRelease()
        {
            _endTouchPosition = _inputBasic.GetTouchPosition();
            DetectSwipe();
        }

        private void DetectSwipe()
        {
            var currentSwipe = new Vector2(_endTouchPosition.x - _startTouchPosition.x,
                _endTouchPosition.y - _startTouchPosition.y);
            currentSwipe.Normalize();

            SendSwipe(_swipeDirectionsService.FindSwipe(currentSwipe));
        }

        private void SendSwipe(SwipeDirection swipeDirection)
        {
            var swipeData = GetSwipeData(swipeDirection);
            OnSwipe?.Invoke(swipeData);
        }

        private SwipeData GetSwipeData(SwipeDirection direction)
        {
            var swipeData = new SwipeData()
            {
                Direction = direction,
                StartPosition = _startTouchPosition,
                EndPosition = _endTouchPosition
            };
            return swipeData;
        }

    }
}