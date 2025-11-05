using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

  private int _summonTicks;
  private bool _hasBlockedOnce;
  private bool _isShooting;
  private Coroutine _shootRoutine;
  private new void Start() {
    base.Start();
    _health = GetComponent<BossHealth>();
    Ticker.Instance.OnTickEvent += CheckState;
    _summonTicks = 20;
  }

  protected override void UpdateGoal() {
    if (dead) return;
    
    switch (state) {
      case BossState.DEAD:
        return;
      case BossState.BLOCKING:
        HandleBlockingPhase();
        HandleShootingPhase();
        break;
      case BossState.WALKING:
        HandleShootingPhase();
        agent.isStopped = false;
        agent.destination = player.position + transform.forward * shootingRange / 2;
        break;
      case BossState.RUNNING:
        HandleShootingPhase();
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
      _health.healthSlider.SetColor(new Color(0, 185, 255));
      return;
    }
    
    if (GameManager.Instance.shield.IsActive()) {
      GameManager.Instance.shield.enabled = false;
    }
    _health.healthSlider.SetColor(Color.red);
    if (_health.isBlocking) {
      _health.isBlocking = false;
    }

    if (distance < runningRange) state = BossState.RUNNING;
    else state = BossState.WALKING;
  }

  #region Phase Logic
  private void HandleBlockingPhase() {
    if (dead) return;

    _health.isBlocking = true;
    agent.isStopped = false;
    agent.destination = player.position + transform.forward * maxDistance / 2;
    _summonTicks++;
    if (_summonTicks == ticksTillSummon) {
      WaveManager.instance.StartNewWave(GameManager.Instance.currentRoom.enemySpawnLocations, GameManager.Instance.currentRoom, true);
      _summonTicks = 0;
    }
    
    if (!_health.isBlocking) {
      agent.isStopped = false;
      state = BossState.WALKING;
    }
  }

  private void HandleShootingPhase() {
    agent.isStopped = false;
    agent.destination = player.position + transform.forward * shootingRange / 2;
    ticks++;
    if (Mathf.Approximately(ticks, shootInterval)) {
      Shoot();
      ticks = 0;
    }
  }

  private void HandleRunningPhase() {
    agent.isStopped = false;
    agent.destination = -player.position;
  }
  #endregion

  #region Shooting
  

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
    b.transform.LookAt(player.position);
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
