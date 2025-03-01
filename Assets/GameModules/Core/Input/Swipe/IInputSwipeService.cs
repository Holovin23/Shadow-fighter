using System;
using UnityEngine;

namespace TFPlay.Core.Input
{
    public interface IInputSwipeService
    {
        public event Action<SwipeData> OnSwipe;
    }
}