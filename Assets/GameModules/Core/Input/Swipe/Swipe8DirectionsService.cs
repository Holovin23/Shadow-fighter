using UnityEngine;

namespace TFPlay.Core.Input
{
    public class Swipe8DirectionsService : ISwipeDirectionsService
    {
        protected class GetCardinalDirections
        {
            public static readonly Vector2 Up = new Vector2(0, 1);
            public static readonly Vector2 Down = new Vector2(0, -1);
            public static readonly Vector2 Right = new Vector2(1, 0);
            public static readonly Vector2 Left = new Vector2(-1, 0);

            public static readonly Vector2 UpRight = new Vector2(1, 1);
            public static readonly Vector2 UpLeft = new Vector2(-1, 1);
            public static readonly Vector2 DownRight = new Vector2(1, -1);
            public static readonly Vector2 DownLeft = new Vector2(-1, -1);
        }

        public SwipeDirection FindSwipe(Vector2 currentSwipe)
        {
            var direction = SwipeDirection.None;
            if (Vector2.Dot(currentSwipe, GetCardinalDirections.Up) > 0.906f)
            {
                direction = SwipeDirection.Up;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Down) > 0.906f)
            {
                direction = SwipeDirection.Down;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Left) > 0.906f)
            {
                direction = SwipeDirection.Left;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.Right) > 0.906f)
            {
                direction = SwipeDirection.Right;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpRight) > 0.906f)
            {
                direction = SwipeDirection.UpRight;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.UpLeft) > 0.906f)
            {
                direction = SwipeDirection.UpLeft;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownLeft) > 0.906f)
            {
                direction = SwipeDirection.DownLeft;
            }
            else if (Vector2.Dot(currentSwipe, GetCardinalDirections.DownRight) > 0.906f)
            {
                direction = SwipeDirection.DownRight;
            }

            return direction;
        }
    }
}