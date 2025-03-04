 using UnityEngine;

 public class RangedPlayerAnimatorController : AnimatorController
 {
     public override void SetSpeed(float speed)
     {
         animator.SetFloat("Speed", speed);
     }

     public override void SetSpeedTwoDimension(Vector2 value)
     {
         animator.SetFloat("MoveX", value.x);
         animator.SetFloat("MoveY", value.y);
     }
    
     public override void SetCombat(bool isInCombat)
     {
         animator.SetBool("IsInCombat", isInCombat);
     }

 }
