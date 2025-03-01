using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TFPlay.Core.Input
{
    public class InputDragService : IInputDragService
    {
        public event Action<Vector2> OnDragAction;

        private IInputBasicService _inputBasicService;
    
        private bool _isDragging;
    
        public InputDragService(IInputBasicService inputBasicService)
        {
            _inputBasicService = inputBasicService;

            _inputBasicService.OnTouch += OnTouch;
            _inputBasicService.OnRelease += OnRelease;
        }

        private void OnTouch()
        {
            _isDragging = true;
            Drag();
        }

        private void OnRelease()
        {
            _isDragging = false;
        }
    
        private async void Drag()
        {
            var startTouchingPosition = _inputBasicService.GetTouchPosition();
            var currentTouchingPosition = startTouchingPosition;
            Vector2 deltaVector;
            Vector2 dragVector;

            while (_isDragging)
            {
                await UniTask.NextFrame();
                currentTouchingPosition = _inputBasicService.GetTouchPosition();
                deltaVector = currentTouchingPosition - startTouchingPosition;

                if (deltaVector.magnitude > 0 && _isDragging)
                {
                    dragVector = new Vector2(deltaVector.x / Screen.width, deltaVector.y / Screen.height);
                    OnDragAction?.Invoke(dragVector);
                    startTouchingPosition = currentTouchingPosition;
                }
            }
        }
    }
}