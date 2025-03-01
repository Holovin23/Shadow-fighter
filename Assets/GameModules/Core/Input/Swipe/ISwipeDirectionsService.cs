using UnityEngine;

namespace TFPlay.Core.Input
{
    public interface ISwipeDirectionsService
    {
        public SwipeDirection FindSwipe(Vector2 currentSwipe);
    }
}