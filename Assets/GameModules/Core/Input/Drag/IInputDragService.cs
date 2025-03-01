using System;
using UnityEngine;

namespace TFPlay.Core.Input
{
    public interface IInputDragService
    {
        public event Action<Vector2> OnDragAction;
    }
}