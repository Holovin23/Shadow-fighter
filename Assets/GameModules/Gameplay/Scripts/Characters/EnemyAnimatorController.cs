using UnityEngine;

public class EnemyAnimatorController : AnimatorController
{
    public override void SetSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public override void SetSpeedTwoDimension(Vector2 value)
    {
       
    }

}
