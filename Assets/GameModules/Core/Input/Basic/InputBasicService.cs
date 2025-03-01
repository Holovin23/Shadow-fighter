using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TFPlay.Core.Input
{
    public class InputBasicService : IInputBasicService
    {
        public event Action OnTouch;
        public event Action OnRelease;

        private DefaultInputs _defaultInputs;

        public InputBasicService()
        {
            CreateInputs();
            SubscribeButtons();

            Enable();
        }
        
        ~InputBasicService()
        {
            UnsubscribeButtons();
        }

        public void Enable()
        {
            _defaultInputs.Enable();
        }

        public void Disable()
        {
            _defaultInputs.Disable();
        }
        
        public Vector2 GetTouchPosition()
        {
            return _defaultInputs.Touch.PrimaryPosition.ReadValue<Vector2>();
        }

        private void CreateInputs()
        {
            _defaultInputs = new DefaultInputs();
        }

        private void SubscribeButtons()
        {
            _defaultInputs.Touch.PrimaryTouch.started += TouchPrimary;
            _defaultInputs.Touch.PrimaryTouch.canceled += ReleasePrimary;
        }

        private void UnsubscribeButtons()
        {
            _defaultInputs.Touch.PrimaryTouch.started -= TouchPrimary;
            _defaultInputs.Touch.PrimaryTouch.canceled -= ReleasePrimary;
        }

        private void TouchPrimary(InputAction.CallbackContext context)
        {
            OnTouch?.Invoke();
        }

        private void ReleasePrimary(InputAction.CallbackContext context)
        {
            OnRelease?.Invoke();
        }
    }
}