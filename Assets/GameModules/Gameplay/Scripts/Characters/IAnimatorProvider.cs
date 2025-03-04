using UnityEngine;

public interface IAnimatorProvider
{
    void SetSpeed(float value);
    void SetSpeedTwoDimension(Vector2 value);
    void StartAttack();
    void SetStun();
    void SetCombat(bool isInCombat);
}
