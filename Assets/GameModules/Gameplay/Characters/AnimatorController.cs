using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour, IAnimatorProvider
{
    [SerializeField] private Animator animator;

    public void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void StartAttack()
    {
        //throw new System.NotImplementedException();
    }

    public void SetStun()
    {
        //throw new System.NotImplementedException();
    }
}
