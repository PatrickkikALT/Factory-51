using System;
using UnityEngine;


public class BossEnemy : Enemy {
  public Animator animator;
  private bool _forward;
  private bool _left;
  private bool _right;
  private bool _backwards;
  public BossState state;
  private BossHealth _health;
  public bool dead;

  [SerializeField] private int blockingStateHealth;

  private new void Start() {
    base.Start();
    _health = GetComponent<BossHealth>();
    Ticker.Instance.OnTickEvent += CheckState;
  }

  protected override void UpdateGoal() {
    if (dead) return;

    switch (state) {
      case BossState.WALKING:
        agent.destination = player.position;
        break;
      case BossState.BLOCKING:
        _health.isBlocking = true;
        break;
      case BossState.SHOOTING:
      case BossState.RUNNING:
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    
    UpdateAnimation();
  }

  private void CheckState() {
    float distance = Vector3.Distance(agent.nextPosition, player.position);

    state = _health.health >= blockingStateHealth ? BossState.BLOCKING :
      distance < 2f ? BossState.RUNNING :
      distance < 10f ? BossState.SHOOTING :
      BossState.WALKING;
  }


  #region Animation
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
  #endregion
  protected override void Shoot() {
    var pos = shootPos.position;

    GameObject obj;
    if (PoolManager.TryDequeue(BulletType.BOSS, out obj)) {
      obj.SetActive(true);
      obj.transform.position = pos;
    }
    else {
      obj = Instantiate(bullet, pos, transform.rotation);
    }
    var b = obj.GetComponent<Bullet>();
    b.direction = Vector3.RotateTowards(transform.forward, player.position, 0.0f, 0.0f);
    b.gameObject.layer = gameObject.layer;
    b.type = BulletType.BOSS;
  }
}