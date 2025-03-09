using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

public interface IAnimatorProvider
{
    public void Play(AnimationData animationData);
    public void Play(string stateName, float transitionTime = 0.1f, TransitionType transitionType = TransitionType.NormalizedTime, int layer = 0);
    void SetSpeed(float value);
    void SetSpeedTwoDimension(Vector2 value);
    void StartAttack();
    void StopAttack();
    void SetStun();
    void SetCombat(bool isInCombat);
}
