
using GameModules.Gameplay.Scripts.Characters;
using UnityEngine;

public abstract class AnimatorController : MonoBehaviour, IAnimatorProvider
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected AnimationData _attackAnimation;

    public virtual void Play(AnimationData data) => Play(data.name, data.transitionTime, data.transitionType, data.layer);

    public void Play(string stateName, float transitionTime = 0.1f,
        TransitionType transitionType = TransitionType.NormalizedTime,
        int layer = 0)
    {
        switch (transitionType)
        {   
            case TransitionType.NormalizedTime:
                animator.CrossFade(stateName, transitionTime, layer);
                break;
            case TransitionType.FixedTime:
                animator.CrossFadeInFixedTime(stateName, transitionTime, layer);
                break;
        }
    }

    public abstract void SetSpeed(float speed);

    public abstract void SetSpeedTwoDimension(Vector2 value);

    public virtual void StartAttack(){}
    
    public virtual void StopAttack(){}
    public virtual void SetStun()
    {
        animator.SetTrigger("Stun");
    }
    
    public virtual void SetCombat(bool isInCombat)
    { }
}
