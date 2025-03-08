using UnityEngine;

 public class MeleePlayerAnimatorController : AnimatorController
 {
     public override void SetSpeed(float speed)
     {
         animator.SetFloat("Speed", speed);
     }

     public override void SetSpeedTwoDimension(Vector2 value)
     {
     }
        
     public override void StartAttack()
     {
         // animator.SetTrigger("Attack");
     }
     
}
