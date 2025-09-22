using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour {
  [Header("AI Navigation")] [SerializeField]
  protected NavMeshAgent agent;

  public Transform player;

  [Header("Attack variables")] [SerializeField]
  protected float maxDistance;

  [SerializeField] protected GameObject bullet;

  [Tooltip("Amount of ticks until shot")] [SerializeField]
  protected int shootSpeed;

  [SerializeField] protected int walkSpeed;

  protected int ticks;

  protected void Start() {
    agent.speed = walkSpeed;
    player = GameManager.Instance.player;
    Ticker.Instance.OnTickEvent += UpdateGoal;
  }

  public void OnDestroy() {
    Ticker.Instance.OnTickEvent -= UpdateGoal;
  }

  protected virtual void UpdateGoal() {
    var distance = Vector3.Distance(player.position, transform.position);
    agent.destination = player.position + -player.forward * maxDistance;
    ticks++;
    if (ticks == shootSpeed) {
      transform.LookAt(player.position);
      Shoot();
      ticks = 0;
    }
    // if (distance <= maxDistance) {
    //   agent.destination = player.position;
    //   Shoot();
    // }
  }


  protected void Shoot() {
    var pos = transform.position + transform.forward;
    var b = Instantiate(bullet, pos, transform.rotation).GetComponent<Bullet>();
    b.direction = Vector3.RotateTowards(transform.forward, player.position, 0.0f, 0.0f);
    b.gameObject.layer = gameObject.layer;
    b.type = BulletType.ENEMY;
  }
}