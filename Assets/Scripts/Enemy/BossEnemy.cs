using UnityEngine;

public class BossEnemy : CircleEnemy {
  public Animator animator;
  private bool _forward;
  private bool _left;
  private bool _right;
  private bool _backwards;

  public bool dead;
  protected override void UpdateGoal() {
    if (dead) return;
    base.UpdateGoal();
    UpdateAnimation();
  }

  private void UpdateAnimation() {
    if (!animator || !agent) return;

    var localVelocity = transform.InverseTransformDirection(agent.velocity);
    var speed = localVelocity.magnitude;
    
    if (speed < 0.1f) {
      animator.SetBool("Forward", false);
      animator.SetBool("Backward", false);
      animator.SetBool("TurnLeft", false);
      animator.SetBool("TurnRight", false);
      return;
    }
    
    if (Mathf.Abs(localVelocity.z) > Mathf.Abs(localVelocity.x)) {
      if (animator.GetBool("Forward") != localVelocity.z > 0f) {
        animator.SetBool("Forward", localVelocity.z > 0f);
      }
      if (animator.GetBool("Backward") != localVelocity.z < 0f) {
        animator.SetBool("Backward", localVelocity.z < 0f);
      }
      animator.SetBool("TurnLeft", false);
      animator.SetBool("TurnRight", false);
    }
    else {
      if (animator.GetBool("TurnLeft") != localVelocity.x < 0f) {
        animator.SetBool("TurnLeft", localVelocity.x < 0f);
      }

      if (animator.GetBool("TurnRight") != localVelocity.x > 0f) {
        animator.SetBool("TurnRight", localVelocity.x > 0f);
      }
      animator.SetBool("Forward", false);
      animator.SetBool("Backward", false);
    }
  }
}