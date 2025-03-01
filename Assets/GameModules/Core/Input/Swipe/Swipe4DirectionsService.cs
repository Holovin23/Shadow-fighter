using UnityEngine;

namespace TFPlay.Core.Input
{
    public class Swipe4DirectionsService : ISwipeDirectionsService
    {
        protected class GetCardinalDirections
        {
            public static readonly Vector2 Up = new Vector2(0, 1);
            public static readonly Vector2 Down = new Vector2(0, -1);
            public static readonly Vector2 Right = new Vector2(1, 0);
            public static readonly Vector2 Left = new Vector2(-1, 0);
        }

        public SwipeDirection FindSwipe(Vector2 currentSwipe)
        {
            var direction = SwipeDirection.None;
            if (Vector2.Dot(currentSwipe, GetCardinalDirections.Up) > 0.707f)
            {
                direction = SwipeDirection.Up;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Down) > 0.707f)
            {
                direction = SwipeDirection.Down;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Left) > 0.707f)
            {
                direction = SwipeDirection.Left;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Right) > 0.707f)
            {
                direction = SwipeDirection.Right;
            }

            return direction;
        }
    }
}