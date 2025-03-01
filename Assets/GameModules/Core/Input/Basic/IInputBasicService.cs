using System;
using UnityEngine;

namespace TFPlay.Core.Input
{
    public interface IInputBasicService
    {
        public event Action OnTouch;
        public event Action OnRelease;
        public Vector2 GetTouchPosition();
    }
}