using System;
using UnityEngine;

public interface IMovementProvider
{
    public event Action<float> OnSpeed;
    public event Action<Vector2> OnSpeedXY;
    
    void Init();
}
