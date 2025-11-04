using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossEnemy : Enemy {
  public Animator animator;
  public BossState state;
  private BossHealth _health;
  public bool dead;

  [SerializeField] private int blockingStateHealth = 200;
  [SerializeField] private float shootInterval = 1.5f;
  [SerializeField] private float runningRange = 2f;
  [SerializeField] private float shootingRange = 10f;
  [SerializeField] private int ticksTillSummon;

  private bool _hasBlockedOnce;
  private bool _isShooting;
  private Coroutine _shootRoutine;

  private new void Start() {
    base.Start();
    _health = GetComponent<BossHealth>();
    Ticker.Instance.OnTickEvent += CheckState;
  }

  protected override void UpdateGoal() {
    if (dead) return;

    switch (state) {
      case BossState.DEAD:
        break;
      case BossState.BLOCKING:
        HandleBlockingPhase();
        break;
      case BossState.WALKING:
        agent.isStopped = false;
        agent.destination = player.position + transform.forward * shootingRange;
        break;
      case BossState.SHOOTING:
        HandleShootingPhase();
        break;
      case BossState.RUNNING:
        HandleRunningPhase();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }

    UpdateAnimation();
  }

  private void CheckState() {
    if (dead) return;

    float distance = Vector3.Distance(agent.nextPosition, player.position);
    
    if (_health.health >= blockingStateHealth) {
      state = BossState.BLOCKING;
      return;
    }

    if (distance < runningRange) state = BossState.RUNNING;
    else if (distance < shootingRange) state = BossState.SHOOTING;
    else state = BossState.WALKING;
  }

  #region Phase Logic
  private void HandleBlockingPhase() {
    if (dead) return;

    _health.isBlocking = true;
    agent.isStopped = true;
    ticks++;
    if (ticks == ticksTillSummon) {
      WaveManager.instance.StartNewWave(GameManager.Instance.currentRoom.enemySpawnLocations, GameManager.Instance.currentRoom, true);
    }
    
    if (!_health.isBlocking) {
      agent.isStopped = false;
      state = BossState.WALKING;
    }
  }

  private void HandleShootingPhase() {
    agent.isStopped = true;
    StartCoroutine(ShootingRoutine());
  }

  private void HandleRunningPhase() {
    agent.isStopped = false;
    agent.destination = -player.position;
  }
  #endregion

  #region Shooting
  private IEnumerator ShootingRoutine() {
    while (_isShooting && !dead) {
      Shoot();
      yield return new WaitForSeconds(shootInterval);
    }
    _isShooting = false;
  }
  

  protected override void Shoot() {
    if (dead) return;

    var pos = shootPos.position;
    GameObject obj;

    if (PoolManager.TryDequeue(BulletType.BOSS, out obj)) {
      obj.SetActive(true);
      obj.transform.position = pos;
    } else {
      obj = Instantiate(bullet, pos, transform.rotation);
    }

    var b = obj.GetComponent<Bullet>();
    b.direction = (player.position - transform.position).normalized;
    b.gameObject.layer = gameObject.layer;
    b.type = BulletType.BOSS;
  }
  #endregion

  #region Animation
  private void UpdateAnimation() {
    var localVelocity = transform.InverseTransformDirection(agent.velocity);
    var speed = localVelocity.magnitude;

    animator.SetBool("Forward", speed > 0.1f && localVelocity.z > 0f);
    animator.SetBool("Backward", speed > 0.1f && localVelocity.z < 0f);
    animator.SetBool("TurnLeft", speed > 0.1f && localVelocity.x < 0f);
    animator.SetBool("TurnRight", speed > 0.1f && localVelocity.x > 0f);
  }
  #endregion

  private new void OnDestroy() {
    Ticker.Instance.OnTickEvent -= CheckState;
    Ticker.Instance.OnTickEvent -= UpdateGoal;
  }
}
