using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatorController : MonoBehaviour, IAnimatorProvider
{
    [SerializeField] protected Animator animator;

    public abstract void SetSpeed(float speed);

    public abstract void SetSpeedTwoDimension(Vector2 value);

    public virtual void StartAttack()
    {
       // animator.SetTrigger("Attack");
    }

    public virtual void SetStun()
    {
        animator.SetTrigger("Stun");
    }
    
    public virtual void SetCombat(bool isInCombat)
    { }
}
